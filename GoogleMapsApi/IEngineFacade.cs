using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using HttpClientUtility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{

    public interface IEngineFacade<in TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        /// <summary>
        /// Occurs when the Url created. Can be used for override the Url.
        /// </summary>
        event UriCreatedDelegate OnUriCreated;

        /// <summary>
        /// Occurs when raw data from Google API recivied.
        /// </summary>
        event RawResponseReceivedDelegate OnRawResponseRecivied;

        /// <summary>
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="service">Http Client Service Instance</param>
        /// <param name="token">Optional cancellation token instance</param>
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        Task<TResponse> QueryAsync(TRequest request, IHttpClientService service, CancellationToken token = default);

        /// <summary>
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="service">Http Client Service instance</param>
        /// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
        /// <param name="token"/>Optional Cancellation Token instance</param>
        /// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
        /// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
        Task<TResponse> QueryAsync(TRequest request, IHttpClientService service, TimeSpan timeout, CancellationToken token = default);
    }
}