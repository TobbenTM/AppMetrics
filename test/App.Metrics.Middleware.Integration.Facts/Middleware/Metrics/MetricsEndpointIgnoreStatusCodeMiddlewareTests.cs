﻿// <copyright file="MetricsEndpointIgnoreStatusCodeMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Middleware.Integration.Facts.Middleware.Metrics
{
    public class MetricsEndpointIgnoreStatusCodeMiddlewareTests : IClassFixture<MetricsHostTestFixture<IgnoredStatusCodeTestStartup>>
    {
        public MetricsEndpointIgnoreStatusCodeMiddlewareTests(MetricsHostTestFixture<IgnoredStatusCodeTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_ignore_specified_status_codes()
        {
            var unauthorizedResponse = await Client.GetAsync("/api/test/401");
            unauthorizedResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var badResponse = await Client.GetAsync("/api/test/400");
            badResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringResult = await result.Content.ReadAsStringAsync();
            stringResult.Should().NotContain("GET api/test/401");
            stringResult.Should().Contain("GET api/test/400");
        }
    }
}