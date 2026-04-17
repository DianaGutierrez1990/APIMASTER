namespace APIMASTER.Data.Entities;

public class MilkDairy
{
    public Guid? ID_Dairy { get; set; }
    public Guid? ID_Customer { get; set; }
    public string? Dairy { get; set; }
    public string? Customer { get; set; }
    public int? Loads { get; set; }
    public decimal? Total_Tons { get; set; }
    public DateTime? Last_Load { get; set; }
    public int? Minutes { get; set; }
    public int? Today_Loads { get; set; }
    public decimal? Today_Tons { get; set; }
}
