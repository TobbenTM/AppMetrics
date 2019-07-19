// <copyright file="MetricsSqlServerOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.SQLServer
{
    public class MetricsSqlServerOutputFormatter : IMetricsOutputFormatter
    {
        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.sqlserver", "v1", "plain");

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; }

        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            throw new NotImplementedException();
        }
    }
}
