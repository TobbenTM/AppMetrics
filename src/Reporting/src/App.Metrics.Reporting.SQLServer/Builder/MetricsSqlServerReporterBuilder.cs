// <copyright file="MetricsTextFileReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.SQLServer;

// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics text file reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsSqlServerReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="SqlServerMetricsReporter" /> allowing metrics to be reported to a SQL Server
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The SQL Server reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToSqlServer(
            this IMetricsReportingBuilder reportingBuilder,
            MetricsReportingSqlServerOptions options)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var reporter = new SqlServerMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="SqlServerMetricsReporter" /> allowing metrics to be reported to a SQL Server
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The SQL Server reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToSqlServer(
            this IMetricsReportingBuilder reportingBuilder,
            Action<MetricsReportingSqlServerOptions> setupAction)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingSqlServerOptions();

            setupAction?.Invoke(options);

            var reporter = new SqlServerMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }
    }
}
