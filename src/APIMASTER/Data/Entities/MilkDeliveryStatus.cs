namespace APIMASTER.Data.Entities;

public class MilkDeliveryStatus
{
    public Guid? ID_Customer_Location { get; set; }
    public string? Date_String { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? Customer { get; set; }
    public int? Loads { get; set; }
    public int? OnRoad_Loads { get; set; }
    public int? Delivered_Loads { get; set; }
    public int? Delivered_Loads_Today { get; set; }
    public int? OnRoad_Loads_Today { get; set; }
    public double? Avg_Delivered_RoadTime { get; set; }
    public double? Avg_OnRoad_RoadTime { get; set; }
    public double? NW_Delivered_Customer_loads { get; set; }
    public double? NW_Delivered_Dairy_Loads { get; set; }
    public double? NW_Delivered_PerDay { get; set; }
    public double? NW_Goal { get; set; }
    public double? NW_Goal_Progress { get; set; }
    public double? NW_Goal_Weight_Progress { get; set; }
    public double? NW_PerDay_Goal { get; set; }
    public double? Loads_PerDay_Goal { get; set; }
    public double? Loads_PerDay { get; set; }
    public double? NW_Delivered { get; set; }
    public double? Today_Loads { get; set; }
    public double? Delivered_Progress { get; set; }
}
