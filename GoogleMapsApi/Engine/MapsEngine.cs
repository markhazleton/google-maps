using GoogleMapsApi.Entities.Common;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Concrete implementation of the MapsAPIGenericEngine for specific types of requests and responses.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request specific to a Google Maps API service.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected from the Google Maps API service.</typeparam>
    public class MapsEngine<TRequest, TResponse> : MapsAPIGenericEngine<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        internal RawResponseReceivedDelegate OnRawResponseRecivied;
        internal new event UriCreatedDelegate OnUriCreated;

        /// <summary>
        /// Additional properties or methods specific to this engine can be defined here.
        /// </summary>

        // Example of an additional method specific to this MapsEngine
        public async Task<TResponse> QueryWithCustomLogicAsync(TRequest request, HttpClient client, CancellationToken cancellationToken = default)
        {
            // Implement any additional logic before querying the Google API
            PreQueryCustomLogic(request);

            // Use the base class's method to perform the actual query
            TimeSpan timeout = TimeSpan.FromSeconds(100); // Example timeout, adjust as needed
            TResponse response = await QueryGoogleAPIAsync(request, client, timeout, cancellationToken);

            // Implement any logic after receiving the response
            PostQueryCustomLogic(response);

            return response;
        }
        private void PreQueryCustomLogic(TRequest request)
        {
            // Implement any logic that needs to run before the query
            // Example: Modify the request parameters, log information, etc.
        }

        private void PostQueryCustomLogic(TResponse response)
        {
            // Implement any logic that needs to run after the query
            // Example: Process the response, log information, handle errors, etc.
        }
    }
}
