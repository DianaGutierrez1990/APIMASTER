namespace APIMASTER.Models.Requests;

public class DateRangeRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
