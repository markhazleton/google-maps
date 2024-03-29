using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientUtility;
/// <summary>
/// Interface for HttpClientService
/// </summary>
public interface IHttpClientService
{
    /// <summary>
    /// Set the timeout for the HttpClient
    /// </summary>
    TimeSpan Timeout { set; }
    /// <summary>
    /// Process Http Get request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HttpResponseContent<T>> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken);
}