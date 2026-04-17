namespace APIMASTER.Models.Requests;

public class MilkLoadsSearchRequest : PaginationParams
{
    public string? Ticket { get; set; }
    public string? Manifest { get; set; }
}
