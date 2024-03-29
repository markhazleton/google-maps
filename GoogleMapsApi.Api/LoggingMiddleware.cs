namespace GoogleMapsApi
{
    /// <summary>
    /// Middleware for logging incoming requests and outgoing responses.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger for logging.</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware asynchronously.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log the incoming request
                LogRequest(context.Request);

                // Call the next middleware in the pipeline
                await _next(context);

                // Log the outgoing response
                LogResponse(context.Response);
            }
            catch (Exception ex)
            {
                // Log exceptions if any
                LogException(ex, context.Request);
                throw; // Ensure the exception is still thrown after logging
            }
        }

        /// <summary>
        /// Logs the incoming request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        private void LogRequest(HttpRequest request)
        {
            // Use a static message template with placeholders
            _logger.LogInformation("Request: {RequestMethod} {RequestPath}{RequestQueryString}",
                request.Method,
                request.Path,
                request.QueryString);
        }

        /// <summary>
        /// Logs the outgoing response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        private void LogResponse(HttpResponse response)
        {
            _logger.LogInformation("Response: {ResponseStatusCode}",
                response.StatusCode);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="request">The HTTP request.</param>
        private void LogException(Exception exception, HttpRequest request)
        {
            // Use Serilog for logging
            _logger.LogError(exception, "Exception: {exceptionMessage} on {requestPath}", exception.Message, request.Path);
        }
    }
}
