namespace FastEndpointApi.endpoints.Geocode
{

    /// <summary>
    /// Represents a request to geocode with a lat/long.
    /// </summary>
    public class GeocodeLatLongRequest
    {
        /// <summary>
        /// Gets or sets the latitude to be geocoded.
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude to be geocoded.
        /// </summary>
        public double Longitude { get; set; }
    }
}
