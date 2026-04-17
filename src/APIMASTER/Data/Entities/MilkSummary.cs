namespace APIMASTER.Data.Entities;

public class MilkSummary
{
    public Guid? ID_Customer { get; set; }
    public string? Customer { get; set; }
    public int? YearNo { get; set; }
    public int? Date_Range { get; set; }
    public string? Date_String { get; set; }
    public int? Loads { get; set; }
    public decimal? Tons { get; set; }
    public DateTime? First_Load { get; set; }
    public DateTime? Last_Load { get; set; }
    public int? Trucks { get; set; }
    public int? Dairies { get; set; }
    public decimal? Avg_TonsLoad { get; set; }
    public int? Avg_TimeIn { get; set; }
    public decimal? Delivered_Percent { get; set; }
    public int? Delivered_Loads { get; set; }
}
