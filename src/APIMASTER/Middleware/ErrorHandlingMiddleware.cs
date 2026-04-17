using System.Net;
using System.Text.Json;

namespace APIMASTER.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var requestId = context.TraceIdentifier;

        var (statusCode, title, detail) = exception switch
        {
            ArgumentException argEx => (
                HttpStatusCode.BadRequest,
                "Invalid argument",
                argEx.Message),
            UnauthorizedAccessException => (
                HttpStatusCode.Forbidden,
                "Access denied",
                "You do not have permission to perform this action."),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Resource not found",
                "The requested resource was not found."),
            InvalidOperationException opEx => (
                HttpStatusCode.UnprocessableEntity,
                "Invalid operation",
                opEx.Message),
            _ => (
                HttpStatusCode.InternalServerError,
                "Internal server error",
                _env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred. Please try again later.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception for request {RequestId}: {Path}",
                requestId, context.Request.Path);
        }
        else
        {
            _logger.LogWarning("Handled exception for request {RequestId}: {ExceptionType} - {Message}",
                requestId, exception.GetType().Name, exception.Message);
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new
        {
            type = $"https://api.tuempresa.com/errors/{title.ToLowerInvariant().Replace(' ', '-')}",
            title,
            status = (int)statusCode,
            detail,
            requestId
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsJsonAsync(problemDetails, options);
    }
}
