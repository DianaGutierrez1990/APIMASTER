namespace APIMASTER.Models.Requests;

public class MilkDairiesRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int LocationId { get; set; }
}
