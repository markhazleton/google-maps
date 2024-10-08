﻿using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests;


[TestFixture]
public class DistanceMatrixTests : BaseTestIntegration
{

    [Test]
    public async Task ShouldReturnValidValueWhenOneOriginAndOneDestinationsSpeciefed()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"]
        };

        var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

        AssertInconclusive.NotExceedQuota(result);
        Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
        CollectionAssert.AreEqual(
            new[] { "Alter Sirksfelder Weg 10, 23881 Koberg, Germany" },
            result.DestinationAddresses);
        CollectionAssert.AreEqual(
            new[] { "St2154 18, 92726 Waidhaus, Germany" },
            result.OriginAddresses);
        Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
        Assert.IsNotNull(result.Rows.First().Elements.First().Distance);
        Assert.IsNotNull(result.Rows.First().Elements.First().Duration);
    }

    [Test]
    public async Task ShouldReturnValidValueWhenTwoOriginsSpecified()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Origins = ["49.64265,12.50088", "49.17395,12.87028"],
            Destinations = ["53.64308,10.52726"]
        };

        var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

        AssertInconclusive.NotExceedQuota(result);
        Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
        CollectionAssert.AreEqual(
            new[] { "Alter Sirksfelder Weg 10, 23881 Koberg, Germany" },
            result.DestinationAddresses);
        CollectionAssert.AreEqual(
            new[] { "St2154 18, 92726 Waidhaus, Germany", "Böhmerwaldstraße 19, 93444 Bad Kötzting, Germany" },
            result.OriginAddresses);
        Assert.AreEqual(2, result.Rows.Count());
        Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
        Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.Last().Elements.First().Status);
    }

    [Test]
    public async Task ShouldReturnDurationInTrafficWhenDepartureTimeAndApiKeySpecified()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            DepartureTime = new Time(),
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

        AssertInconclusive.NotExceedQuota(result);
        Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);

        Assert.IsNotNull(result.Rows.First().Elements.First().DurationInTraffic);
    }

    [Test]
    public void ShouldThrowExceptionWhenDepartureTimeAndArrivalTimeSpecified()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            DepartureTime = new Time(),
            ArrivalTime = new Time(),
            Mode = DistanceMatrixTravelModes.transit,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public void ShouldThrowExceptionWhenArrivalTimeSpecifiedForNonTransitModes()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            ArrivalTime = new Time(),
            Mode = DistanceMatrixTravelModes.driving,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public void ShouldThrowExceptionWheTransitRoutingPreferenceSpecifiedForNonTransitModes()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Mode = DistanceMatrixTravelModes.driving,
            TransitRoutingPreference = DistanceMatrixTransitRoutingPreferences.less_walking,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public void ShouldThrowExceptionWhenTrafficModelSuppliedForNonDrivingMode()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Mode = DistanceMatrixTravelModes.transit,
            DepartureTime = new Time(),
            TrafficModel = DistanceMatrixTrafficModels.optimistic,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public void ShouldThrowExceptionWhenTrafficModelSuppliedWithoutDepartureTime()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Mode = DistanceMatrixTravelModes.driving,
            TrafficModel = DistanceMatrixTrafficModels.optimistic,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public void ShouldThrowExceptionWhenTransitModesSuppliedForNonTransitMode()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Mode = DistanceMatrixTravelModes.driving,
            TransitModes = [DistanceMatrixTransitModes.bus, DistanceMatrixTransitModes.subway],
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        Assert.ThrowsAsync<ArgumentException>(() => GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService));
    }

    [Test]
    public async Task ShouldReturnImperialUnitsIfImperialPassedAsParameter()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Units = DistanceMatrixUnitSystems.imperial,
            Origins = ["49.64265,12.50088"],
            Destinations = ["53.64308,10.52726"],
        };

        var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

        AssertInconclusive.NotExceedQuota(result);
        Assert.True(result.Rows.First().Elements.First().Distance.Text.Contains("mi"));
    }

    [Test]
    [Ignore("Need to fix it")]
    public async Task ShouldReplaceUriViaOnUriCreated()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Origins = ["placeholder"],
            Destinations = ["3,4"],
        };

        static Uri onUriCreated(Uri uri)
        {
            var builder = new UriBuilder(uri);
            builder.Query = builder.Query.Replace("placeholder", "1,2");
            return builder.Uri;
        }

        GoogleMaps.DistanceMatrix.OnUriCreated += onUriCreated;

        try
        {
            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual("1,2", result.OriginAddresses.First());
        }
        finally
        {
            GoogleMaps.DistanceMatrix.OnUriCreated -= onUriCreated;
        }
    }

    [Test]
    [Ignore("Need to fix it")]
    public async Task ShouldPassRawDataToOnRawResponseRecivied()
    {
        var request = new DistanceMatrixRequest
        {
            ApiKey = ApiKey,
            Origins = ["placeholder"],
            Destinations = ["3,4"],
        };

        var rawData = Array.Empty<byte>();

        void onRawResponseRecivied(byte[] data) => rawData = data;
        GoogleMaps.DistanceMatrix.OnRawResponseRecivied += onRawResponseRecivied;

        try
        {
            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request, _httpClientService);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            CollectionAssert.IsNotEmpty(rawData);
        }
        finally
        {
            GoogleMaps.DistanceMatrix.OnRawResponseRecivied -= onRawResponseRecivied;
        }
    }
}
