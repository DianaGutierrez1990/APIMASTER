namespace APIMASTER.Models.Requests;

public class CrossUserLoadsRequest : PaginationParams
{
    public int UserId { get; set; }
    public int DaysBefore { get; set; } = 7;
}
