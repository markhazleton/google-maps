namespace FastEndpointApi.endpoints.Geocode;

/// <summary>
/// Represents a request to geocode with an address.
/// </summary>
public class GeocodeAddressRequest
{
    /// <summary>
    /// Gets or sets the address to be geocoded.
    /// </summary>
    public string? Address { get; set; }
}
