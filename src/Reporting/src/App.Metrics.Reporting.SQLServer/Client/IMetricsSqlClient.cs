// <copyright file="IMetricsSqlClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace App.Metrics.Reporting.SQLServer.Client
{
    public interface IMetricsSqlClient
    {
        Task EnsureDatabase();

        Task EnsureTable();

        Task LogMetric();
    }
}
