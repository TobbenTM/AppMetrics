// <copyright file="IMetricsSqlClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Formatters.SQLServer.Internal;

namespace App.Metrics.Reporting.SQLServer.Client
{
    public interface IMetricsSqlClient
    {
        Task EnsureTable();

        Task<SqlWriteResult> WriteAsync(SqlMetricRow[] rows);
    }
}
