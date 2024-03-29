namespace GoogleMapsApi
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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

        private void LogRequest(HttpRequest request)
        {
            // Use a static message template with placeholders
            _logger.LogInformation("Request: {RequestMethod} {RequestPath}{RequestQueryString}",
                request.Method,
                request.Path,
                request.QueryString);
        }

        private void LogResponse(HttpResponse response)
        {
            _logger.LogInformation("Response: {ResponseStatusCode}",
                response.StatusCode);
        }

        private void LogException(Exception exception, HttpRequest request)
        {
            // Use Serilog for logging
            _logger.LogError(exception, "Exception: {exceptionMessage} on {requestPath}", exception.Message, request.Path);
        }
    }
}
