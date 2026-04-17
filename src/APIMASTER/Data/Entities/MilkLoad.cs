namespace APIMASTER.Data.Entities;

public class MilkLoad
{
    public Guid? ID_Weight { get; set; }
    public Guid? ID_Dairy { get; set; }
    public Guid? ID_Delivery_Location { get; set; }
    public Guid? ID_Truck { get; set; }
    public Guid? ID_Hauler { get; set; }
    public string? Dairy { get; set; }
    public string? Delivery_Location { get; set; }
    public string? ScaleTicket { get; set; }
    public DateTime? Load_Date { get; set; }
    public DateTime? DateIn { get; set; }
    public DateTime? DateOut { get; set; }
    public int? Minutes { get; set; }
    public int? WeightIn { get; set; }
    public int? WeightOut { get; set; }
    public decimal? NWTon { get; set; }
    public string? Manifest { get; set; }
    public long? ManifestNo { get; set; }
    public string? RouteNo { get; set; }
    public string? BarnNo { get; set; }
    public string? Silo { get; set; }
    public string? TruckNo { get; set; }
    public string? Hauler { get; set; }
    public string? Delivered_Status { get; set; }
    public DateTime? Delivered_Date { get; set; }
    public int? TimeIn_Dairy { get; set; }
    public int? Road_Time { get; set; }
    public int? Downlod_Time { get; set; }
    public string? Delivery_Truck { get; set; }
    public string? Delivery_Trailer { get; set; }
    public string? BOLImage { get; set; }
}
