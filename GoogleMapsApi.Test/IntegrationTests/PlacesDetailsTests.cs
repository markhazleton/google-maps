using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestClass]
    public class PlacesDetailsTests : BaseTestIntegration
    {
        [TestMethod]
        public async Task ReturnsPhotos()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Result.Photos.Any());
        }

        [TestMethod]
        public async Task ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.NOT_FOUND, result.Status);
        }

        readonly PriceLevel[] anyPriceLevel = [PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive];

        [TestMethod]
        public async Task ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceId(),
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotNull(result.Result.PriceLevel);
            Assert.IsTrue(anyPriceLevel.Contains(result.Result.PriceLevel.Value));
        }

        [TestMethod]
        public async Task ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceId(),
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);

            // commented out because seems like google doesn't have opening hours for this place anymore
            /*
            Assert.AreEqual(7, result.Result.OpeningHours.Periods.Count());
            var sundayPeriod = result.Result.OpeningHours.Periods.First();
            Assert.AreEqual(DayOfWeek.Sunday, sundayPeriod.OpenTime.Day);
            Assert.IsTrue(sundayPeriod.OpenTime.Time >= 0);
            Assert.IsTrue(sundayPeriod.OpenTime.Time <= 2359);
            Assert.IsTrue(sundayPeriod.CloseTime.Time >= 0);
            Assert.IsTrue(sundayPeriod.CloseTime.Time <= 2359);
             */
        }

        private string? cachedMyPlaceId;
        private async Task<string> GetMyPlaceId()
        {
            if (cachedMyPlaceId == null)
            {
                var request = new Entities.Places.Request.PlacesRequest()
                {
                    ApiKey = ApiKey,
                    Name = "My Place Bar & Restaurant",
                    Location = new Location(-31.954453, 115.862717),
                    RankBy = Entities.Places.Request.RankBy.Distance,
                };
                var result = await GoogleMaps.Places.QueryAsync(request, _httpClientService);
                AssertInconclusive.NotExceedQuota(result);
                cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return cachedMyPlaceId;
        }
    }
}
