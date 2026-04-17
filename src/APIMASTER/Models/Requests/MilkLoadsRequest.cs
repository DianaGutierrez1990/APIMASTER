namespace APIMASTER.Models.Requests;

public class MilkLoadsRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DairyId { get; set; }
    public int CustomerLocationId { get; set; }
}
