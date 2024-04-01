using GoogleMapsApi.Entities.Common;
using HttpClientUtility;
using HttpClientUtility.StringConverter;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReceivedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        private static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);
        public static event UriCreatedDelegate OnUriCreated;
        //public static event RawResponseReceivedDelegate OnRawResponseReceived;
        private HttpClient _client;

        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, IHttpClientService clientService, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            var httpResponse = await clientService.GetAsync<TResponse>(uri, token);
            if (httpResponse.IsSuccess)
            {
                return httpResponse.Content;
            }
            throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode} and message: {httpResponse.Content}");
        }
        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, IHttpClientService clientService, TimeSpan timeout, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            clientService.Timeout = timeout;
            var httpResponse = await clientService.GetAsync<TResponse>(uri, token);
            if (httpResponse.IsSuccess)
            {
                return httpResponse.Content;
            }
            throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode} and message: {httpResponse.Content}");
        }


        protected internal async Task<TResponse> QueryGoogleAPIAsync(TRequest request, HttpClient client, TimeSpan timeout, CancellationToken token = default)
        {
            _client = client;
            var converter = new NewtonsoftJsonStringConverter();
            ArgumentNullException.ThrowIfNull(request);
            var uri = request.GetUri();
            OnUriCreated?.Invoke(uri);
            var response = await GetHttpResponseContent(uri, timeout, token).ConfigureAwait(false);
            return converter.ConvertFromString<TResponse>(response);
        }
        private async Task<string> GetHttpResponseContent( Uri address, TimeSpan timeout, CancellationToken token)
        {
            var httpResponse = await GetHttpResponse(address, timeout, token).ConfigureAwait(false);
            return await httpResponse.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        }
        private async Task<HttpResponseMessage> GetHttpResponse( Uri address, TimeSpan timeout, CancellationToken token)
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
    }
}
