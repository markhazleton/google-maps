using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace GoogleMapsApi.Api;

/// <summary>
/// Provides methods to register and access Google Maps APIs.
/// </summary>
public static class RegisterGoogleMapsApi
{
    /// <summary>
    /// RegisterGoogleMapsAPIs
    /// </summary>
    /// <param name="app"></param>
    public static void RegisterGoogleMapsAPIs(this WebApplication app)
    {
        app.MapGet("/geocoding/address", (IConfiguration configuration, string address) =>
        {
            return GetGeoCodingFromAddress(configuration, address);
        });
        app.MapGet("/geocoding/latlong", (IConfiguration configuration, double latitude, double longitude) =>
        {
            return GetGeocodingFromLatLong(configuration, latitude,longitude);
        });
    }
    private static async Task<GeocodingResponse> GetGeocodingFromLatLong(IConfiguration app, double latitude, double longitude)
    {
        string apiKey = app.GetValue<string>("GOOGLE_API_KEY") ?? "not found";
        var request = new GeocodingRequest
        {
            ApiKey = apiKey,
            Location = new Location(latitude, longitude)
        };
        return await GoogleMaps.Geocode.QueryAsync(request);
    }
    private static async Task<GeocodingResponse> GetGeoCodingFromAddress(IConfiguration app, string address)
    {
        string apiKey = app.GetValue<string>("GOOGLE_API_KEY") ?? "not found";
        var request = new GeocodingRequest
        {
            ApiKey = apiKey,
            Address = address
        };
        return await GoogleMaps.Geocode.QueryAsync(request);
    }
}

