using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientUtility;

/// <summary>
/// Represents a service for making HTTP requests. This class encapsulates the functionality of an HTTP client and provides
/// methods to perform asynchronous GET requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the HttpClientService class with the specified HTTP client factory and string converter.
/// </remarks>
/// <param name="httpClientFactory">The factory to create HTTP client instances.</param>
/// <param name="stringConverter">The converter to deserialize response strings.</param>
/// <exception cref="ArgumentNullException">Thrown when httpClientFactory or stringConverter is null.</exception>
public class HttpClientService(IHttpClientFactory httpClientFactory, HttpClientUtility.IStringConverter stringConverter) : HttpClientUtility.IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    private readonly HttpClientUtility.IStringConverter _stringConverter = stringConverter ?? throw new ArgumentNullException(nameof(stringConverter));
    private TimeSpan? _timeout;

    /// <summary>
    /// Sets the timeout for the HTTP client. If not set, defaults to 100 seconds.
    /// </summary>
    public TimeSpan Timeout
    {
        set => _timeout = value;
    }

    /// <summary>
    /// Asynchronously sends a GET request to the specified Uri and returns the response body as a type specified by T.
    /// </summary>
    /// <typeparam name="T">The type to which the response body will be converted.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the deserialized response.</returns>
    public async Task<HttpResponseContent<T>> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken = default)
    {
        // Ensure a default timeout is set if none is specified
        var client = _httpClientFactory.CreateClient();
        client.Timeout = _timeout ?? TimeSpan.FromSeconds(100);

        try
        {
            var response = await client.GetAsync(requestUri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var deserializedContent = _stringConverter.ConvertFromString<T>(content);
                return new HttpResponseContent<T>(deserializedContent, response.StatusCode);
            }
            else
            {
                // Handle non-success status codes
                return new HttpResponseContent<T>($"Error: {response.ReasonPhrase}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle known exceptions related to HTTP requests
            return new HttpResponseContent<T>($"HTTP Request Exception: {ex.Message}", HttpStatusCode.ServiceUnavailable);
        }
        catch (TaskCanceledException ex)
        {
            // Handle timeout exception
            return new HttpResponseContent<T>($"Timeout Exception: {ex.Message}", HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions
            return new HttpResponseContent<T>($"Unexpected Exception: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }
    /// <summary>
    /// Asynchronously sends a POST request to the specified Uri with the specified payload and returns the deserialized response body.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <typeparam name="TResult">The type to which the response body will be converted.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="payload">The payload to send in the request body.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the deserialized response.</returns>
    public async Task<HttpResponseContent<TResult>> PostAsync<T, TResult>(Uri requestUri, T payload, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        // Ensure a default timeout is set if none is specified
        client.Timeout = _timeout ?? TimeSpan.FromSeconds(100);

        try
        {
            // Serialize the payload to a JSON string
            var jsonPayload = stringConverter.ConvertFromModel(payload);
            using var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, httpContent, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var deserializedContent = _stringConverter.ConvertFromString<TResult>(content);
                return new HttpResponseContent<TResult>(deserializedContent, response.StatusCode);
            }
            else
            {
                // Handle non-success status codes
                return new HttpResponseContent<TResult>($"Error: {response.ReasonPhrase}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle known exceptions related to HTTP requests
            return new HttpResponseContent<TResult>($"HTTP Request Exception: {ex.Message}", HttpStatusCode.ServiceUnavailable);
        }
        catch (TaskCanceledException ex)
        {
            // Handle timeout exception
            return new HttpResponseContent<TResult>($"Timeout Exception: {ex.Message}", HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions
            return new HttpResponseContent<TResult>($"Unexpected Exception: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }
    /// <summary>
    /// Asynchronously sends a PUT request to the specified Uri with the specified payload.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <typeparam name="TResult">The type to which the response body will be converted.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="payload">The payload to send in the request body.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the deserialized response.</returns>
    public async Task<HttpResponseContent<TResult>> PutAsync<T, TResult>(Uri requestUri, T payload, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = _timeout ?? TimeSpan.FromSeconds(100);

        try
        {
            var jsonPayload = stringConverter.ConvertFromModel(payload);
            using var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(requestUri, httpContent, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var deserializedContent = _stringConverter.ConvertFromString<TResult>(content);
                return new HttpResponseContent<TResult>(deserializedContent, response.StatusCode);
            }
            else
            {
                return new HttpResponseContent<TResult>($"Error: {response.ReasonPhrase}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            return new HttpResponseContent<TResult>($"HTTP Request Exception: {ex.Message}", HttpStatusCode.ServiceUnavailable);
        }
        catch (TaskCanceledException ex)
        {
            return new HttpResponseContent<TResult>($"Timeout Exception: {ex.Message}", HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            return new HttpResponseContent<TResult>($"Unexpected Exception: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }
    /// <summary>
    /// Asynchronously sends a DELETE request to the specified Uri.
    /// </summary>
    /// <typeparam name="TResult">The type to which the response body will be converted.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the deserialized response.</returns>
    public async Task<HttpResponseContent<TResult>> DeleteAsync<TResult>(Uri requestUri, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = _timeout ?? TimeSpan.FromSeconds(100);

        try
        {
            var response = await client.DeleteAsync(requestUri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var deserializedContent = _stringConverter.ConvertFromString<TResult>(content);
                return new HttpResponseContent<TResult>(deserializedContent, response.StatusCode);
            }
            else
            {
                return new HttpResponseContent<TResult>($"Error: {response.ReasonPhrase}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            return new HttpResponseContent<TResult>($"HTTP Request Exception: {ex.Message}", HttpStatusCode.ServiceUnavailable);
        }
        catch (TaskCanceledException ex)
        {
            return new HttpResponseContent<TResult>($"Timeout Exception: {ex.Message}", HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            return new HttpResponseContent<TResult>($"Unexpected Exception: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

}
