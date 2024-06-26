﻿using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Status = GoogleMapsApi.Entities.Geocoding.Response.Status;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class GeocodingTests : BaseTestIntegration
    {
        [Test]
        public async Task Geocoding_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            StringAssert.IsMatch("40\\.\\d*,-73\\.\\d*", result.Results.First().Geometry.Location.LocationString);
        }

        [Test]
        public async Task GeocodingAsync_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            StringAssert.IsMatch("40\\.\\d*,-73\\.\\d*", result.Results.First().Geometry.Location.LocationString);
        }

        [Test]
        public void Geocoding_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ApiKey = ApiKey, ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

            Assert.ThrowsAsync<HttpRequestException>(() => GoogleMaps.Geocode.QueryAsync(request, _httpClientService));
        }

        [Test]
        public void GeocodingAsync_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<HttpRequestException>(),
                          () => GoogleMaps.Geocode.QueryAsync(request, _httpClientService).Wait());
        }

        [Test]
        public void GeocodingAsync_Cancel_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

            var tokeSource = new CancellationTokenSource();
            var task = GoogleMaps.Geocode.QueryAsync(request, _httpClientService, tokeSource.Token);
            tokeSource.Cancel();

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<HttpRequestException>(),
                () => task.Wait());
        }

        [Test]
        public void GeocodingAsync_WithPreCanceledToken_Cancels()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var task = GoogleMaps.Geocode.QueryAsync(request, _httpClientService, cts.Token);

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<HttpRequestException>(),
                            () => task.Wait());
        }

        [Test]
        public async Task ReverseGeocoding_LatLng_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Location = new Location(40.7141289, -73.9614074)
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReverseGeocoding_PlaceId_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJo9YpQWBZwokR7OeY0hiWh8g"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReverseGeocoding_PlaceIdAndRegion_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJo9YpQWBZwokR7OeY0hiWh8g",
                Region = "US"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReverseGeocoding_PlaceIdAndBounds_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJo9YpQWBZwokR7OeY0hiWh8g",
                Bounds =
                [
                    new Location(40.7154070802915, -73.9599636697085),
                    new Location(40.7127091197085, -73.96266163029151)
                ]
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReverseGeocoding_PlaceIdAndComponents_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJo9YpQWBZwokR7OeY0hiWh8g",
                Components = new() { Country = "US" }
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReverseGeocoding_PlaceIdAndAddress_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJo9YpQWBZwokR7OeY0hiWh8g",
                Address = "Should be ignored"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public void ReverseGeocodingAsync_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Location = new Location(40.7141289, -73.9614074)
            };

            var result = GoogleMaps.Geocode.QueryAsync(request, _httpClientService).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }
    }
}