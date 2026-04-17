namespace APIMASTER.Models.Requests;

public class CommoditySummaryRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Group { get; set; } = "Month";
}
