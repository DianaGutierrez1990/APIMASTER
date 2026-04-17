namespace APIMASTER.Data.Entities;

public class MilkCustomer
{
    public Guid? ID_Delivery_Location { get; set; }
    public string? Delivery_Location { get; set; }
    public int? Loads { get; set; }
    public decimal? NWTons { get; set; }
    public DateTime? Last_Load { get; set; }
    public int? Minutes { get; set; }
    public int? Today_Loads { get; set; }
    public decimal? Today_Tons { get; set; }
}
