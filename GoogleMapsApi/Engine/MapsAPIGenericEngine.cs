using GoogleMapsApi.Entities.Common;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebSpark.HttpClientUtility.RequestResult;
using WebSpark.HttpClientUtility.StringConverter;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Delegate for URI creation events
    /// </summary>
    /// <param name="uri">The created URI</param>
    /// <returns>The URI</returns>
    public delegate Uri UriCreatedDelegate(Uri uri);
    /// <summary>
    /// Delegate for raw response received events
    /// </summary>
    /// <param name="data">The raw response data</param>
    public delegate void RawResponseReceivedDelegate(byte[] data);

    /// <summary>
    /// Abstract base class for Google Maps API engines that handle HTTP requests and responses.
    /// Provides core functionality for making HTTP requests to Google Maps APIs with proper error handling,
    /// logging, and timeout management following Azure best practices.
    /// </summary>
    /// <typeparam name="TRequest">The request type that inherits from MapsBaseRequest</typeparam>
    /// <typeparam name="TResponse">The response type that implements IResponseFor</typeparam>
    public abstract class MapsAPIGenericEngine<TRequest, TResponse> where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        /// <summary>
        /// Infinite timeout value used to indicate no timeout should be applied
        /// </summary>
        private static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// Event fired when a URI is created for a Google Maps API request.
        /// Can be used for logging, debugging, or request monitoring purposes.
        /// </summary>
        public static event UriCreatedDelegate OnUriCreated;

        //public static event RawResponseReceivedDelegate OnRawResponseReceived;

        /// <summary>
        /// HTTP client instance for making requests
        /// </summary>
        private HttpClient _client;

        /// <summary>
        /// Logger instance for structured logging
        /// </summary>
        private readonly ILogger _logger;        // Ensure logger is passed to derived classes
        /// <summary>
        /// Initializes a new instance of the <see cref="MapsAPIGenericEngine{TRequest, TResponse}"/> class.
        /// Sets up the engine with required dependencies for making Google Maps API requests.
        /// </summary>
        /// <param name="logger">The logger instance to use for structured logging throughout the request lifecycle.</param>
        /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
        protected MapsAPIGenericEngine(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }        /// <summary>
                 /// Executes a Google Maps API request using the provided client service.
                 /// Handles the complete request lifecycle including URI creation, HTTP execution,
                 /// response processing, error handling, and structured logging.
                 /// </summary>
                 /// <param name="request">The request object containing API parameters and configuration.</param>
                 /// <param name="clientService">The HTTP request result service that handles the actual HTTP communication.</param>
                 /// <param name="token">A cancellation token to cancel the operation if needed.</param>
                 /// <returns>The deserialized response object of type TResponse.</returns>
                 /// <exception cref="ArgumentNullException">Thrown when request parameter is null.</exception>
                 /// <exception cref="UnauthorizedAccessException">Thrown when API key is invalid or access is forbidden.</exception>
                 /// <exception cref="InvalidOperationException">Thrown for rate limiting or service unavailability.</exception>
                 /// <exception cref="ArgumentException">Thrown for invalid request parameters.</exception>
                 /// <exception cref="TimeoutException">Thrown when the request times out.</exception>
                 /// <exception cref="GoogleMapsApiException">Thrown for other API-specific errors.</exception>
        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, IHttpRequestResultService clientService, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            var req = new HttpRequestResult<TResponse>
            {
                RequestPath = uri.ToString(),
                RequestMethod = HttpMethod.Get,
            };

            var result = await clientService.HttpSendRequestResultAsync<TResponse>(req, ct: token);
            if (result.IsSuccessStatusCode && result.ResponseResults != null)
            {
                _logger.LogInformation("[GoogleMapsApi] Query succeeded. StatusCode: {StatusCode}, CorrelationId: {CorrelationId}, Duration: {DurationMs}ms", result.StatusCode, result.CorrelationId, result.RequestDurationMilliseconds);
                return result.ResponseResults;
            }
            var errorSummary = (result.ErrorList != null && result.ErrorList.Count > 0) ? string.Join(", ", result.ErrorList) : "none";
            _logger.LogError("[GoogleMapsApi] Query failed. StatusCode: {StatusCode}, Errors: [{Errors}], CorrelationId: {CorrelationId}, Duration: {DurationMs}ms", result.StatusCode, errorSummary, result.CorrelationId, result.RequestDurationMilliseconds);
            throw CreateSpecificException(result.StatusCode, errorSummary, result.CorrelationId, result.RequestDurationMilliseconds);
        }        /// <summary>
                 /// Executes a Google Maps API request using the provided client service with a specified timeout.
                 /// Handles the complete request lifecycle including URI creation, HTTP execution,
                 /// response processing, error handling, and structured logging with timeout management.
                 /// </summary>
                 /// <param name="request">The request object containing API parameters and configuration.</param>
                 /// <param name="clientService">The HTTP request result service that handles the actual HTTP communication.</param>
                 /// <param name="timeout">The maximum time to wait for the request to complete.</param>
                 /// <param name="token">A cancellation token to cancel the operation if needed.</param>
                 /// <returns>The deserialized response object of type TResponse.</returns>
                 /// <exception cref="ArgumentNullException">Thrown when request parameter is null.</exception>
                 /// <exception cref="UnauthorizedAccessException">Thrown when API key is invalid or access is forbidden.</exception>
                 /// <exception cref="InvalidOperationException">Thrown for rate limiting or service unavailability.</exception>
                 /// <exception cref="ArgumentException">Thrown for invalid request parameters.</exception>
                 /// <exception cref="TimeoutException">Thrown when the request times out.</exception>
                 /// <exception cref="GoogleMapsApiException">Thrown for other API-specific errors.</exception>
        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, IHttpRequestResultService clientService, TimeSpan timeout, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            var req = new HttpRequestResult<TResponse>
            {
                RequestPath = uri.ToString(),
                RequestMethod = HttpMethod.Get,
            };

            var result = await clientService.HttpSendRequestResultAsync<TResponse>(req, ct: token);
            if (result.IsSuccessStatusCode && result.ResponseResults != null)
            {
                _logger.LogInformation("[GoogleMapsApi] Query succeeded. StatusCode: {StatusCode}, CorrelationId: {CorrelationId}, Duration: {DurationMs}ms", result.StatusCode, result.CorrelationId, result.RequestDurationMilliseconds);
                return result.ResponseResults;
            }
            var errorSummary = (result.ErrorList != null && result.ErrorList.Count > 0) ? string.Join(", ", result.ErrorList) : "none";
            _logger.LogError("[GoogleMapsApi] Query failed. StatusCode: {StatusCode}, Errors: [{Errors}], CorrelationId: {CorrelationId}, Duration: {DurationMs}ms", result.StatusCode, errorSummary, result.CorrelationId, result.RequestDurationMilliseconds);
            throw CreateSpecificException(result.StatusCode, errorSummary, result.CorrelationId, result.RequestDurationMilliseconds);
        }        /// <summary>
                 /// Executes a Google Maps API request using a raw HttpClient with specified timeout.
                 /// This method provides lower-level access to HTTP operations for scenarios requiring
                 /// direct HttpClient control while maintaining consistent error handling and logging.
                 /// </summary>
                 /// <param name="request">The request object containing API parameters and configuration.</param>
                 /// <param name="client">The HttpClient instance to use for the HTTP request.</param>
                 /// <param name="timeout">The maximum time to wait for the request to complete.</param>
                 /// <param name="token">A cancellation token to cancel the operation if needed.</param>
                 /// <returns>The deserialized response object of type TResponse.</returns>
                 /// <exception cref="ArgumentNullException">Thrown when request parameter is null.</exception>
                 /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
                 /// <exception cref="ArgumentOutOfRangeException">Thrown when timeout value is invalid.</exception>
        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, HttpClient client, TimeSpan timeout, CancellationToken token = default)
        {
            _client = client;
            var converter = new NewtonsoftJsonStringConverter();
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            var response = await GetHttpResponseContent(uri, timeout, token).ConfigureAwait(false); return converter.ConvertFromString<TResponse>(response);
        }

        /// <summary>
        /// Gets the HTTP response content as a string from the specified URI.
        /// This is a helper method that handles the HTTP response reading process.
        /// </summary>
        /// <param name="address">The URI to request content from.</param>
        /// <param name="timeout">The maximum time to wait for the request.</param>
        /// <param name="token">A cancellation token to cancel the operation.</param>
        /// <returns>The response content as a string.</returns>
        private async Task<string> GetHttpResponseContent(Uri address, TimeSpan timeout, CancellationToken token)
        {
            var httpResponse = await GetHttpResponse(address, timeout, token).ConfigureAwait(false); return await httpResponse.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the HTTP response message from the specified URI with timeout handling.
        /// Validates the response for success status codes and throws appropriate exceptions for failures.
        /// </summary>
        /// <param name="address">The URI to send the HTTP request to.</param>
        /// <param name="timeout">The maximum time to wait for the request.</param>
        /// <param name="token">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when client or address is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when timeout value is invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        private async Task<HttpResponseMessage> GetHttpResponse(Uri address, TimeSpan timeout, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(_client);
            ArgumentNullException.ThrowIfNull(address);
            if (timeout.TotalMilliseconds < 0 && timeout != InfiniteTimeout)
                throw new ArgumentOutOfRangeException(nameof(timeout), "The timeout value must be a positive number or equal to InfiniteTimeout.");

            _client.Timeout = timeout;
            var httpResponse = await _client.GetAsync(address, token).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = await httpResponse.Content.ReadAsStringAsync(token).ConfigureAwait(false);
                throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode} and message: {errorMessage}");
            }
            return httpResponse;
        }

        /// <summary>
        /// Creates specific exceptions based on HTTP status codes following Azure best practices.
        /// Maps HTTP status codes to appropriate .NET exception types for better error handling
        /// and provides detailed error information for debugging and monitoring.
        /// </summary>
        /// <param name="statusCode">The HTTP status code returned from the API request.</param>
        /// <param name="errorSummary">A summary of the errors encountered during the request.</param>
        /// <param name="correlationId">A unique correlation ID for tracking the request across systems.</param>
        /// <param name="requestDuration">The duration of the request in milliseconds for performance monitoring.</param>
        /// <returns>An appropriate exception type based on the status code and error context.</returns>
        private static Exception CreateSpecificException(System.Net.HttpStatusCode? statusCode, string errorSummary, string correlationId, long? requestDuration)
        {
            var baseMessage = $"Request failed with status code {statusCode}. Errors: {errorSummary}";

            return statusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Google Maps API key is invalid or missing. {baseMessage}"),
                System.Net.HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Access forbidden - check API key permissions and billing. {baseMessage}"),
                System.Net.HttpStatusCode.TooManyRequests => new InvalidOperationException($"Rate limit exceeded - too many requests. {baseMessage}"),
                System.Net.HttpStatusCode.BadRequest => new ArgumentException($"Invalid request parameters. {baseMessage}"),
                System.Net.HttpStatusCode.RequestTimeout => new TimeoutException($"Request timed out. {baseMessage}"),
                System.Net.HttpStatusCode.ServiceUnavailable => new InvalidOperationException($"Google Maps API is temporarily unavailable. {baseMessage}"),
                System.Net.HttpStatusCode.InternalServerError => new InvalidOperationException($"Google Maps API internal server error. {baseMessage}"),
                _ => new GoogleMapsApiException(baseMessage, statusCode, correlationId ?? string.Empty, requestDuration)
            };
        }
    }
}
