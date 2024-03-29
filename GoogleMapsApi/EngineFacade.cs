using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using HttpClientUtility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
    /// <summary>
    /// A public-surface API that exposes the Google Maps API functionality.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request.</typeparam>
    /// <typeparam name="TResponse">Type of the response.</typeparam>
    public class EngineFacade<TRequest, TResponse> : IEngineFacade<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        internal static readonly EngineFacade<TRequest, TResponse> Instance = new();

        private readonly MapsEngine<TRequest, TResponse> _mapsEngine;

        private EngineFacade()
        {
            _mapsEngine = new MapsEngine<TRequest, TResponse>();
        }

        /// <summary>
        /// Occurs when the Url created. Can be used for override the Url.
        /// </summary>
        public event UriCreatedDelegate OnUriCreated
        {
            add
            {
                _mapsEngine.OnUriCreated += value;
            }
            remove
            {
                _mapsEngine.OnUriCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when raw data from Google API received.
        /// </summary>
        public event RawResponseReceivedDelegate OnRawResponseRecivied
        {
            add
            {
                _mapsEngine.OnRawResponseRecivied += value;
            }
            remove
            {
                _mapsEngine.OnRawResponseRecivied -= value;
            }
        }

        public Task<TResponse> QueryAsync(TRequest request, IHttpClientService service, CancellationToken token = default)
        {
            return _mapsEngine.QueryGoogleAPIAsync(request, service, token);
        }

        public Task<TResponse> QueryAsync(TRequest request, IHttpClientService service, TimeSpan timeout, CancellationToken token = default)
        {
            return _mapsEngine.QueryGoogleAPIAsync(request, service,timeout, token);
        }
    }
}
