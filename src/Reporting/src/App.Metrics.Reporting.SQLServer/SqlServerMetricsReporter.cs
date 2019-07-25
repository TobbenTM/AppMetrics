// <copyright file="SqlServerMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.SQLServer;
using App.Metrics.Formatters.SQLServer.Internal;
using App.Metrics.Logging;
using App.Metrics.Reporting.SQLServer.Client;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.SQLServer
{
    public class SqlServerMetricsReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<SqlServerMetricsReporter>();
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsSqlServerOutputFormatter();
        private readonly MetricsReportingSqlServerOptions _options;

        public SqlServerMetricsReporter(MetricsReportingSqlServerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingSqlServerOptions.FlushInterval)} must not be less than zero");
            }

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingSqlServerOptions.ConnectionString)} cannot be null or empty");
            }

            Formatter = options.MetricsOutputFormatter ?? _defaultMetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;

            _options = options;

            Logger.Info($"Using Metrics Reporter {this}. Table name: {_options.TableName} FlushInterval: {FlushInterval}");

            EnsureInfrastructure().GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        public async Task EnsureInfrastructure()
        {
            using (var client = new DefaultMetricsSqlClient(_options))
            {
                if (_options.CreateTableIfNotExists)
                {
                    await client.EnsureTable();
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            Logger.Trace("Flushing metrics snapshot");

            using (var stream = new MemoryStream())
            using (var client = new DefaultMetricsSqlClient(_options, cancellationToken))
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                stream.Position = 0;

                SqlMetricRow[] rows = new SqlMetricRow[0];
                var result = await client.WriteAsync(rows);

                if (result.Success)
                {
                    Logger.Trace("Successfully flushed metrics snapshot");
                    return true;
                }

                Logger.Error(result.ErrorMessage);

                return false;
            }
        }
    }
}
