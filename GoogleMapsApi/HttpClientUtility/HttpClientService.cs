using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.HttpClientUtility;

public class HttpClientService(IHttpClientFactory httpClientFactory, IStringConverter stringConverter) : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    private readonly IStringConverter _stringConverter = stringConverter ?? throw new ArgumentNullException(nameof(stringConverter));
    private TimeSpan? _timeout;
    public TimeSpan Timeout { set => _timeout = value; }

    public async Task<HttpResponseContent<T>> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        // check for null timeout, set to 100 seconds if null
        if (_timeout == null)
        {
            _timeout = TimeSpan.FromSeconds(100);
        }
        client.Timeout = (TimeSpan)_timeout;
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
                return new HttpResponseContent<T>($"Error: {response.ReasonPhrase}",response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return new HttpResponseContent<T>($"Exception: {ex.Message}",HttpStatusCode.InternalServerError);
        }
    }
}