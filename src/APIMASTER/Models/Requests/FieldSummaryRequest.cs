namespace APIMASTER.Models.Requests;

public class FieldSummaryRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Crop { get; set; }
    public string? Field { get; set; }
}
