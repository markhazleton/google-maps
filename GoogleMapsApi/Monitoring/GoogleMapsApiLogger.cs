using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using WebSpark.HttpClientUtility.RequestResult;

namespace GoogleMapsApi.Monitoring
{
    /// <summary>
    /// Enhanced logging wrapper for Google Maps API requests following Azure best practices
    /// </summary>
    public class GoogleMapsApiLogger
    {
        private readonly ILogger<GoogleMapsApiLogger> _logger;
        private static readonly ActivitySource ActivitySource = new("GoogleMapsApi");

        public GoogleMapsApiLogger(ILogger<GoogleMapsApiLogger> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }        /// <summary>
                 /// Logs request start with telemetry
                 /// </summary>
        public Activity LogRequestStart(string operation, Uri requestUri)
        {
            var activity = ActivitySource.StartActivity($"GoogleMapsApi.{operation}");
            activity?.SetTag("maps.operation", operation);
            activity?.SetTag("maps.uri", requestUri.ToString());

            _logger.LogInformation("Starting Google Maps API request for {Operation} to {Uri}",
                operation, requestUri);

            return activity;
        }

        /// <summary>
        /// Logs successful request completion
        /// </summary>
        public void LogRequestSuccess(Activity activity, string operation, TimeSpan duration, long? requestBytes = null)
        {
            activity?.SetTag("maps.success", true);
            activity?.SetTag("maps.duration_ms", duration.TotalMilliseconds);

            if (requestBytes.HasValue)
            {
                activity?.SetTag("maps.response_bytes", requestBytes.Value);
            }

            _logger.LogInformation("Google Maps API request {Operation} completed successfully in {Duration}ms",
                operation, duration.TotalMilliseconds);

            activity?.Dispose();
        }        /// <summary>
                 /// Logs request failure with detailed error information
                 /// </summary>
        public void LogRequestFailure(Activity activity, string operation, Exception exception, TimeSpan? duration = null)
        {
            activity?.SetTag("maps.success", false);
            activity?.SetTag("maps.error", exception.GetType().Name);
            activity?.SetTag("maps.error_message", exception.Message);

            if (duration.HasValue)
            {
                activity?.SetTag("maps.duration_ms", duration.Value.TotalMilliseconds);
            }

            _logger.LogError(exception, "Google Maps API request {Operation} failed after {Duration}ms: {Error}",
                operation, duration?.TotalMilliseconds ?? 0, exception.Message);

            activity?.Dispose();
        }        /// <summary>
                 /// Logs rate limit warnings
                 /// </summary>
        public void LogRateLimitWarning(string operation, string quotaInfo = null)
        {
            _logger.LogWarning("Google Maps API rate limit approaching for {Operation}. Quota info: {QuotaInfo}",
                operation, quotaInfo ?? "Unknown");
        }

        /// <summary>
        /// Logs API key validation issues
        /// </summary>
        public void LogApiKeyIssue(string operation, string issue)
        {
            _logger.LogError("Google Maps API key issue for {Operation}: {Issue}", operation, issue);
        }
    }
}
