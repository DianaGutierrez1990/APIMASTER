namespace APIMASTER.Models.Requests;

public class CommodityLoadsRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CommodityId { get; set; }
    public int? VendorId { get; set; }
    public string? DeliveryLocation { get; set; }
}
