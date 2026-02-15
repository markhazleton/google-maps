using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestClass]
    public class ElevationTests : BaseTestIntegration
    {
        [TestMethod]
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

        [TestMethod]
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
