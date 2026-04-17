namespace APIMASTER.Models.Requests;

public class HarvestRequest : PaginationParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? FieldId { get; set; }
    public string? Crop { get; set; }
}
