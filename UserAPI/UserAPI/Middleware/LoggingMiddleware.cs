using System.Diagnostics;

namespace UserAPI.Middleware
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
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("➡️  Incoming Request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);  // call the next middleware
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("⬅️  Response: {StatusCode} ({Elapsed} ms)",
                    context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
