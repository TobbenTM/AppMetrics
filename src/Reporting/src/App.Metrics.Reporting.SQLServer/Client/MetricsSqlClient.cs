// <copyright file="MetricsSqlClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.SQLServer.Client
{
    public sealed class MetricsSqlClient : IMetricsSqlClient, IDisposable
    {
        private static readonly ILog Logger = LogProvider.For<MetricsSqlClient>();
        private const string TableNameScriptKey = "#TableName#";

        private readonly MetricsReportingSqlServerOptions _options;
        private readonly SqlConnection _connection;

        public MetricsSqlClient(MetricsReportingSqlServerOptions options)
        {
            _options = options;
            _connection = new SqlConnection(_options.ConnectionString);

            try
            {
                _connection.Open();
            } catch (SqlException e)
            {
                Logger.Error(e, "Could not connect to SQL Server!");
                throw;
            }
        }

        public Task EnsureDatabase()
        {
            throw new NotImplementedException();
        }

        public async Task EnsureTable()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "App.Metrics.Reporting.SQLServer.Scripts.CreateTable.sql";

#pragma warning disable S3966 // Objects should not be disposed more than once
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
#pragma warning restore S3966 // Objects should not be disposed more than once
            using (StreamReader reader = new StreamReader(stream))
            {
                string script = reader.ReadToEnd();

                var command = _connection.CreateCommand();
                command.CommandText = script.Replace(TableNameScriptKey, _options.TableName);
                await command.ExecuteNonQueryAsync();
            }
        }

        public Task LogMetric()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
