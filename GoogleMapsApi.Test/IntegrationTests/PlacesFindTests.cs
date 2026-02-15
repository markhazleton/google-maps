using GoogleMapsApi.Entities.PlacesFind.Request;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestClass]
    public class PlacesFindTests : BaseTestIntegration
    {
        [TestMethod]
        public async Task ReturnsResults()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "pizza wichita ks",
                InputType = InputType.TextQuery
            };

            PlacesFindResponse result = await GoogleMaps.PlacesFind.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Candidates.Any());
        }

        [TestMethod]
        public async Task DoesNotReturnFieldsWhenNotRequested()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "pizza wichita ks",
                InputType = InputType.TextQuery,
                Fields = "place_id"
            };

            PlacesFindResponse result = await GoogleMaps.PlacesFind.QueryAsync(request, _httpClientService);

            //FormattedAddress should be null since it wasn't requested
            Assert.IsTrue(result.Candidates.Any());
            Assert.IsNull(result.Candidates.FirstOrDefault()?.FormattedAddress);
        }
    }
}
