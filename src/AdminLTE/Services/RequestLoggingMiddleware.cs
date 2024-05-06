using System.Diagnostics;

namespace AdminLTE.Services;

/// <summary>
///     A middleware for logging all requests.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    ///     A constructor for the request logging middleware.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    ///     Method that is invoked upon each request and will log the request information.
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        var watch = Stopwatch.StartNew();
        await _next.Invoke(context);
        watch.Stop();

        var logTemplate = @"Client IP: {clientIP}
                                Request path: {requestPath}
                                Request content type: {requestContentType}
                                Request content length: {requestContentLength}
                                Start time: {startTime}
                                Duration: {duration}";

        _logger.LogInformation(logTemplate,
            context.Connection.RemoteIpAddress?.ToString(),
            context.Request.Path,
            context.Request.ContentType,
            context.Request.ContentLength,
            startTime,
            watch.ElapsedMilliseconds);
    }
}