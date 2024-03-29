using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
    /// <summary>
    /// A public-surface API that exposes the Google Maps API functionality.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class EngineFacade<TRequest, TResponse> : IEngineFacade<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        internal static readonly EngineFacade<TRequest, TResponse> Instance = new();

        private EngineFacade() { }

        /// <summary>
        /// Occurs when the Url created. Can be used for override the Url.
        /// </summary>
        public event UriCreatedDelegate OnUriCreated
        {
            add
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnUriCreated += value;
            }
            remove
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnUriCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when raw data from Google API recivied.
        /// </summary>
        public event RawResponseReciviedDelegate OnRawResponseRecivied
        {
            add
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnRawResponseRecivied += value;
            }
            remove
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnRawResponseRecivied -= value;
            }
        }
        public Task<TResponse> QueryAsync(TRequest request, HttpClient client, TimeSpan timeout, CancellationToken token = default)
        {
            return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request, client, timeout, token);
        }
        public Task<TResponse> QueryAsync(TRequest request, HttpClient client, CancellationToken token = default)
        {
            return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request, client, TimeSpan.FromMilliseconds(Timeout.Infinite), token);
        }
    }
}