using System.Diagnostics;

namespace APIMASTER.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path.Value;
        var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;

            if (IsWriteOperation(method))
            {
                _logger.LogInformation(
                    "AUDIT | {Method} {Path} | Status: {StatusCode} | User: {UserId} | Duration: {Duration}ms",
                    method, path, statusCode, userId, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogDebug(
                    "{Method} {Path} | Status: {StatusCode} | User: {UserId} | Duration: {Duration}ms",
                    method, path, statusCode, userId, stopwatch.ElapsedMilliseconds);
            }
        }
    }

    private static bool IsWriteOperation(string method) =>
        method is "POST" or "PUT" or "PATCH" or "DELETE";
}
