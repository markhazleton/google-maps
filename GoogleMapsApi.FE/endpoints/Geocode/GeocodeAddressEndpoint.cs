using FastEndpoints;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace FastEndpointApi.endpoints.Geocode;
/// <summary>
/// Represents the endpoint for geocoding Latitude/Longitude values
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeocodeLatLongEndpoint"/> class.
/// </remarks>
/// <param name="geocodeService">The geocode service.</param>
/// <param name="logger">The logger.</param>
public class GeocodeLatLongEndpoint(IGeocodeService geocodeService, ILogger<GeocodeLatLongEndpoint> logger) : Endpoint<GeocodeLatLongRequest, GeocodingResponse>
{
    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/geocode/{Latitude}/{Longitude}");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the geocode latlong request asynchronously.
    /// </summary>
    /// <param name="req">The geocode latlong request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(GeocodeLatLongRequest req, CancellationToken ct)
    {
        try
        {
            var geocodeResponse = await geocodeService.GetGeocodingFromLatLong(req.Latitude, req.Longitude);

            if (geocodeResponse == null)
            {
                await SendNotFoundAsync(cancellation: ct);
            }
            else
            {
                await SendAsync(geocodeResponse, cancellation: ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while geocoding the latlong.");
            throw; // Rethrow the exception or handle it as necessary
        }
    }
}


/// <summary>
/// Represents the endpoint for geocoding an address.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeocodeAddressEndpoint"/> class.
/// </remarks>
/// <param name="geocodeService">The geocode service.</param>
/// <param name="logger">The logger.</param>
public class GeocodeAddressEndpoint(IGeocodeService geocodeService, ILogger<GeocodeAddressEndpoint> logger) : Endpoint<GeocodeAddressRequest, GeocodingResponse>
{
    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/geocode/{Address}");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the geocode address request asynchronously.
    /// </summary>
    /// <param name="req">The geocode address request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(GeocodeAddressRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Address))
        {
            ThrowError("Address is required."); // Ensure you have a mechanism to throw meaningful errors
        }

        try
        {
            var geocodeResponse = await geocodeService.GetGeoCodingFromAddress(req.Address);

            if (geocodeResponse == null)
            {
                await SendNotFoundAsync(cancellation: ct);
            }
            else
            {
                await SendAsync(geocodeResponse, cancellation: ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while geocoding the address.");
            throw; // Rethrow the exception or handle it as necessary
        }
    }
}

