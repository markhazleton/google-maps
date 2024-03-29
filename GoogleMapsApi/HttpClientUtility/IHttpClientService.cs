using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.HttpClientUtility
{
    public interface IHttpClientService
    {
        TimeSpan Timeout { set; }

        Task<HttpResponseContent<T>> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken);
    }
}