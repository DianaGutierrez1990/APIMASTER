namespace APIMASTER.Models.Responses;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }

    public static ErrorResponse Validation(
        Dictionary<string, string[]> errors, string requestId) => new()
    {
        Type = "https://api.tuempresa.com/errors/validation",
        Title = "Validation error",
        Status = 400,
        Detail = "One or more fields failed validation.",
        RequestId = requestId,
        Errors = errors
    };

    public static ErrorResponse NotFound(string detail, string requestId) => new()
    {
        Type = "https://api.tuempresa.com/errors/not-found",
        Title = "Resource not found",
        Status = 404,
        Detail = detail,
        RequestId = requestId
    };
}
