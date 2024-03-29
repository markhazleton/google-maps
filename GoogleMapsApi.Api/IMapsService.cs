using GoogleMapsApi.Entities.Geocoding.Response;

namespace GoogleMapsApi.Api
{

    /// <summary>
    /// Represents a service for interacting with the Google Maps API.
    /// </summary>
    public interface IMapsService
    {
        /// <summary>
        /// Retrieves the geocoding information for the specified address.
        /// </summary>
        /// <param name="address">The address to geocode.</param>
        /// <returns>The geocoding response.</returns>
        Task<GeocodingResponse> GetGeoCodingFromAddress(string address);

        /// <summary>
        /// Retrieves the geocoding information for the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>The geocoding response.</returns>
        Task<GeocodingResponse> GetGeocodingFromLatLong(double latitude, double longitude);
    }
}

