namespace APIMASTER.Data.Entities;

public class CrossMilkLoad
{
    public int Id_Milk_Load { get; set; }
    public int? Id_Customer_Dairy_Barn_Silo { get; set; }
    public string? Scale_Ticket { get; set; }
    public string? Manifest { get; set; }
    public string? Truck { get; set; }
    public string? Trailer { get; set; }
    public string? Driver { get; set; }
    public string? Seal { get; set; }
    public decimal? Gross_Weight { get; set; }
    public decimal? Tare_Weight { get; set; }
    public decimal? Net_Weight { get; set; }
    public decimal? Temperature { get; set; }
    public DateTime? Date_In { get; set; }
    public DateTime? Date_Out { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public int? Id_User { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Updated_At { get; set; }
}
