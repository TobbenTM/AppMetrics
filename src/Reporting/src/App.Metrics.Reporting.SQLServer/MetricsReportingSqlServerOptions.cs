// <copyright file="MetricsReportingSqlServerOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting.SQLServer
{
    /// <summary>
    ///     Provides programmatic configuration of SQL Server Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingSqlServerOptions
    {
        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the connection string to use for connections.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the table to propagate with data.
        /// </summary>
        public string TableName { get; set; } = "Metrics";

        /// <summary>
        ///     Gets or sets the flag for auto-creating database.
        /// </summary>
        public bool CreateDatabaseIfNotExists { get; set; } = false;

        /// <summary>
        ///     Gets or sets the flag for auto-creating database table.
        /// </summary>
        public bool CreateTableIfNotExists { get; set; } = false;

        /// <summary>
        ///     Gets or sets the interval between flushing metrics.
        /// </summary>
        public TimeSpan FlushInterval { get; set; }
    }
}
