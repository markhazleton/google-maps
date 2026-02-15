using GoogleMapsApi.Entities.PlacesText.Request;
using GoogleMapsApi.Entities.PlacesText.Response;
using GoogleMapsApi.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestClass]
    public class PlacesTextTests : BaseTestIntegration
    {
        [TestMethod]
        public async Task ReturnsFormattedAddress()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "1 smith st parramatta",
                Types = "address"
            };

            PlacesTextResponse result = await GoogleMaps.PlacesText.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("1 Smith St, Parramatta NSW 2150, Australia", result.Results.First().FormattedAddress);
        }

        [TestMethod]
        public async Task ReturnsPhotos()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "1600 Pennsylvania Ave NW",
                Types = "address"
            };

            PlacesTextResponse result = await GoogleMaps.PlacesText.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotNull(result.Results);
            Assert.IsTrue(result.Results.Any());
        }
    }
}
