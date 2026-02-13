using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Common;
using WebSpark.HttpClientUtility.RequestResult;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.HealthChecks
{
    /// <summary>
    /// Health check for Google Maps API connectivity and API key validation
    /// </summary>
    public class GoogleMapsApiHealthCheck : IHealthCheck
    {
        private readonly IHttpRequestResultService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public GoogleMapsApiHealthCheck(IHttpRequestResultService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _apiKey = _configuration.GetValue<string>("GOOGLE_API_KEY") ?? string.Empty;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if API key is configured
                if (string.IsNullOrWhiteSpace(_apiKey))
                {
                    return HealthCheckResult.Unhealthy("Google Maps API key is not configured");
                }

                // Perform a lightweight test request to validate API connectivity
                var request = new GeocodingRequest
                {
                    ApiKey = _apiKey,
                    // Use a simple, well-known address that should always work
                    Address = "1600 Amphitheatre Parkway, Mountain View, CA"
                };

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await GoogleMaps.Geocode.QueryAsync(request, _httpClientService, cancellationToken);
                stopwatch.Stop();

                if (response?.Status == Entities.Geocoding.Response.Status.OK)
                {
                    return HealthCheckResult.Healthy($"Google Maps API is responsive (Response time: {stopwatch.ElapsedMilliseconds}ms)");
                }
                else if (response?.Status == Entities.Geocoding.Response.Status.REQUEST_DENIED)
                {
                    return HealthCheckResult.Unhealthy("Google Maps API key is invalid or access is denied");
                }
                else if (response?.Status == Entities.Geocoding.Response.Status.OVER_QUERY_LIMIT)
                {
                    return HealthCheckResult.Degraded("Google Maps API quota exceeded");
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"Google Maps API returned unexpected status: {response?.Status}");
                }
            }
            catch (TaskCanceledException)
            {
                return HealthCheckResult.Unhealthy("Google Maps API health check timed out");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Google Maps API health check failed: {ex.Message}", ex);
            }
        }
    }
}
