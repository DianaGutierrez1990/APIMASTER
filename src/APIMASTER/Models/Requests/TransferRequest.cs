namespace APIMASTER.Models.Requests;

public class TransferRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? CommodityId { get; set; }
    public int? DairyId { get; set; }
}
