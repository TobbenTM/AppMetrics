// <copyright file="MetricSnapshotSqlServerWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.SQLServer
{
    public sealed class MetricSnapshotSqlServerWriter : IMetricSnapshotWriter
    {
        public MetricSnapshotSqlServerWriter()
        {
        }

        public void Write(
            string context,
            string name,
            string field,
            object value,
            MetricTags tags,
            DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public void Write(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags,
            DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
