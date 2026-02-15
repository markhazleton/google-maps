using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using GoogleMapsApi.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestClass]
    public class TimeZoneTests : BaseTestIntegration
    {
        [TestMethod]
        [Ignore("Need to fix it")]
        public async Task TimeZone_Correct_OverviewPath()
        {
            var request = new TimeZoneRequest
            {
                ApiKey = ApiKey,
                Location = new Location(55.866413, 12.501063),
                Language = "en"
            };

            TimeZoneResponse result = await GoogleMaps.TimeZone.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
        }
    }
}