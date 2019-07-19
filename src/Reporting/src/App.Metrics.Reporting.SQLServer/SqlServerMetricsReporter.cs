// <copyright file="MetricsTextFileReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.SQLServer;
using App.Metrics.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.SQLServer
{
    public class SqlServerMetricsReporter : IReportMetrics
    {

        private static readonly ILog Logger = LogProvider.For<SqlServerMetricsReporter>();
        private static readonly int _defaultBufferSize = 4096;
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
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
