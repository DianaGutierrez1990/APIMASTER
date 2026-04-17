namespace APIMASTER.Models.Requests;

public class MilkSummaryRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DairyId { get; set; }
    public string DeliveryLocation { get; set; } = string.Empty;

    /// <summary>
    /// Grouping period: Week, Month, Quarter, Year
    /// </summary>
    public string Group { get; set; } = "Month";
}
