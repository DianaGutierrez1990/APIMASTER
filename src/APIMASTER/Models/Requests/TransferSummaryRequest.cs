namespace APIMASTER.Models.Requests;

public class TransferSummaryRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Group { get; set; } = "Month";
    public int? CommodityId { get; set; }
}
