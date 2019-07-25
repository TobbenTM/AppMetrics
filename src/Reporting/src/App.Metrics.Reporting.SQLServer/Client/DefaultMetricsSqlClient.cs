// <copyright file="MetricsSqlClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.SQLServer.Internal;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.SQLServer.Client
{
    public sealed class DefaultMetricsSqlClient : IMetricsSqlClient, IDisposable
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsSqlClient>();

        private static readonly Dictionary<string, string> _scriptCache = new Dictionary<string, string>();
        private readonly MetricsReportingSqlServerOptions _options;
        private readonly CancellationToken _cancellationToken;
        private readonly SqlConnection _connection;

        public DefaultMetricsSqlClient(MetricsReportingSqlServerOptions options, CancellationToken cancellationToken = default)
        {
            _options = options;
            _cancellationToken = cancellationToken;
            _connection = new SqlConnection(_options.ConnectionString);

            try
            {
                _connection.Open();
            }
            catch (SqlException e)
            {
                Logger.Error(e, "Could not connect to SQL Server!");
                throw;
            }
        }

        public async Task EnsureTable()
        {
            var command = _connection.CreateCommand();
            command.CommandText = await GetScript("CreateTable");
            command.Parameters.AddWithValue("TableName", _options.TableName);
            await command.ExecuteNonQueryAsync();
        }

        private async Task<string> GetScript(string scriptName)
        {
            if (_scriptCache.ContainsKey(scriptName))
            {
                return _scriptCache[scriptName];
            }

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().FullName}.Scripts.{scriptName}.sql";

#pragma warning disable S3966 // Objects should not be disposed more than once
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
#pragma warning restore S3966 // Objects should not be disposed more than once
            using (StreamReader reader = new StreamReader(stream))
            {
                var script = await reader.ReadToEndAsync();
                _scriptCache.Add(scriptName, script);
                return script;
            }
        }

        public async Task<SqlWriteResult> WriteAsync(SqlMetricRow[] rows)
        {
            try
            {
                foreach (var row in rows)
                {
                    var command = _connection.CreateCommand();

                    command.CommandText = await GetScript("InsertMetric").ConfigureAwait(false);
                    command.Parameters.AddWithValue("Timestamp", row.Timestamp);
                    command.Parameters.AddWithValue("Context", row.Context);
                    command.Parameters.AddWithValue("Name", row.Name);
                    command.Parameters.AddWithValue("Field", row.Field);
                    command.Parameters.AddWithValue("Value", row.Value);
                    command.Parameters.AddWithValue("Tags", row.Tags);

                    await command.ExecuteNonQueryAsync(_cancellationToken).ConfigureAwait(false);
                }

                return new SqlWriteResult(true);
            }
            catch (Exception e)
            {
                return new SqlWriteResult(false, e.Message);
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
