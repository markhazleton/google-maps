using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using WebSpark.HttpClientUtility.RequestResult;

namespace FastEndpointApi.endpoints.Geocode;

/// <summary>
/// Provides methods to interact with Google Maps Geocoding API.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GoogleMapsGeocodeService"/> class.
/// </remarks>
/// <param name="httpClientService">The factory to create instances of <see cref="HttpClient"/>.</param>
/// <param name="configuration">The application configuration to access Google Maps API key.</param>
public class GoogleMapsGeocodeService(IHttpRequestResultService httpClientService, IConfiguration configuration) : IGeocodeService
{
    private readonly string apiKey = configuration.GetValue<string>("GOOGLE_API_KEY") ?? "not found";

    /// <summary>
    /// Retrieves geocoding information based on latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>A <see cref="Task{GeocodingResponse}"/> representing the asynchronous operation, containing the geocoding response.</returns>
    public async Task<GeocodingResponse> GetGeocodingFromLatLong(double latitude, double longitude)
    {
        // Construct the request for the Google Maps Geocoding API.
        var request = new GeocodingRequest
        {
            ApiKey = apiKey,
            Location = new Location(latitude, longitude)
        };

        // Execute the geocoding query using the Google Maps service and return the response.
        return await GoogleMaps.Geocode.QueryAsync(request, httpClientService);
    }

    /// <summary>
    /// Retrieves geocoding information based on a physical address.
    /// </summary>
    /// <param name="address">The address to geocode.</param>
    /// <returns>A <see cref="Task{GeocodingResponse}"/> representing the asynchronous operation, containing the geocoding response.</returns>
    public async Task<GeocodingResponse> GetGeoCodingFromAddress(string address)
    {
        // Construct the request for the Google Maps Geocoding API using the provided address.
        var request = new GeocodingRequest
        {
            ApiKey = apiKey,
            Address = address
        };

        // Execute the geocoding query using the Google Maps service and return the response.
        return await GoogleMaps.Geocode.QueryAsync(request, httpClientService);
    }
}

