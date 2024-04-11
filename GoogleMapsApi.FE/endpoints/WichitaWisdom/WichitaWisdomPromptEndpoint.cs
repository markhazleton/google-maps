using FastEndpoints;
using GoogleMapsApi.FE.endpoints.WichitaWisdom.Service;

namespace GoogleMapsApi.FE.endpoints.WichitaWisdom;
/// <summary>
/// Represents the endpoint for geocoding Latitude/Longitude values
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WichitaWisdomPromptEndpoint"/> class.
/// </remarks>
/// <param name="wisdomService">The geocode service.</param>
/// <param name="logger">The logger.</param>
public class WichitaWisdomPromptEndpoint(IWichitaWisdomService wisdomService, ILogger<WichitaWisdomPromptEndpoint> logger) : Endpoint<WichitaWisdomRequest, string>
{
    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/WichitaWisdom/{Prompt}");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the geocode latlong request asynchronously.
    /// </summary>
    /// <param name="req">The geocode latlong request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(WichitaWisdomRequest req, CancellationToken ct)
    {
        try
        {
            var wisdomResponse = await wisdomService.GetWichitaWisdom(req.Prompt);

            if (wisdomResponse == null)
            {
                await SendNotFoundAsync(cancellation: ct);
            }
            else
            {
                await SendAsync(wisdomResponse, cancellation: ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while geocoding the latlong.");
            throw; // Rethrow the exception or handle it as necessary
        }
    }
}



