﻿using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class ElevationTests : BaseTestIntegration
    {
        [Test]
        public async Task Elevation_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = [new Location(40.7141289, -73.9614074)]
            };

            var result = await GoogleMaps.Elevation.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }

        [Test]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = [new Location(40.7141289, -73.9614074)]
            };

            var result = GoogleMaps.Elevation.QueryAsync(request, _httpClientService).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }
    }
}
