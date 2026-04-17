namespace APIMASTER.Models.Responses;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
    public string RequestId { get; set; } = string.Empty;

    public static PaginatedResponse<T> Create(
        IEnumerable<T> items, int totalCount, int page, int pageSize, string requestId)
    {
        return new PaginatedResponse<T>
        {
            Data = items,
            Pagination = new PaginationMeta
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            },
            RequestId = requestId
        };
    }
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
