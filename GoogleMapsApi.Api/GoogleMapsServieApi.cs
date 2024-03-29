using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.HttpClientUtility;

namespace GoogleMapsApi.Api;

/// <summary>
/// Provides methods to interact with Google Maps Geocoding API.
/// </summary>
public class GoogleMapsServiceApi : IMapsService
{
    private readonly IHttpClientService _httpClientService;
    private readonly string apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleMapsServiceApi"/> class.
    /// </summary>
    /// <param name="httpClientService">The factory to create instances of <see cref="HttpClient"/>.</param>
    /// <param name="configuration">The application configuration to access Google Maps API key.</param>
    public GoogleMapsServiceApi(IHttpClientService httpClientService, IConfiguration configuration)
    {
        // Retrieve the Google Maps API key from configuration.
        // Fallback to "not found" if the key is not present.
        apiKey = configuration.GetValue<string>("GOOGLE_API_KEY") ?? "not found";

        _httpClientService = httpClientService;
    }

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
        return await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);
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
        return await GoogleMaps.Geocode.QueryAsync(request, _httpClientService);
    }
}

