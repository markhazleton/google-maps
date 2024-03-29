using GoogleMapsApi.Entities.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
    using Engine;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;

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
        event RawResponseReciviedDelegate OnRawResponseRecivied;

        /// <summary>
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="client">The HttpClient that will be used to send the request.</param>
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        Task<TResponse> QueryAsync(TRequest request,HttpClient client, CancellationToken token = default);

        /// <summary>
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
        /// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
        /// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
        Task<TResponse> QueryAsync(TRequest request, HttpClient client, TimeSpan timeout,CancellationToken token = default);
    }
}