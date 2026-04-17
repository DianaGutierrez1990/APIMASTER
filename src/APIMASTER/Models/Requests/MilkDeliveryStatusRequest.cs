namespace APIMASTER.Models.Requests;

public class MilkDeliveryStatusRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
}
