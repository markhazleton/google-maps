using System;
using System.Net;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Custom exception for Google Maps API specific errors
    /// </summary>
    public class GoogleMapsApiException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the error
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// Gets the correlation ID for tracking the request
        /// </summary>
        public string CorrelationId { get; }

        /// <summary>
        /// Gets the request duration in milliseconds
        /// </summary>
        public long? RequestDuration { get; }

        /// <summary>
        /// Initializes a new instance of the GoogleMapsApiException class
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="correlationId">The correlation ID</param>
        /// <param name="requestDuration">The request duration in milliseconds</param>
        public GoogleMapsApiException(string message, HttpStatusCode? statusCode = null, string correlationId = "", long? requestDuration = null)
            : base(message)
        {
            StatusCode = statusCode;
            CorrelationId = correlationId ?? string.Empty;
            RequestDuration = requestDuration;
        }

        /// <summary>
        /// Initializes a new instance of the GoogleMapsApiException class with an inner exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="correlationId">The correlation ID</param>
        /// <param name="requestDuration">The request duration in milliseconds</param>
        public GoogleMapsApiException(string message, Exception innerException, HttpStatusCode? statusCode = null, string correlationId = "", long? requestDuration = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            CorrelationId = correlationId ?? string.Empty;
            RequestDuration = requestDuration;
        }
    }
}
