namespace APIMASTER.Models.Responses;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public string RequestId { get; set; } = string.Empty;

    public static ApiResponse<T> Success(T data, string requestId) => new()
    {
        Data = data,
        RequestId = requestId
    };
}
