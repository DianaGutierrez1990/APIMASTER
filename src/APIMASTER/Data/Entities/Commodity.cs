namespace APIMASTER.Data.Entities;

public class CommodityPickup
{
    public int Id_Commodity { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Vendor_Name { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
}

public class CommodityDelivery
{
    public int Id_Commodity { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Delivery_Location { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
}

public class CommodityVendor
{
    public int Id_Vendor { get; set; }
    public string? Vendor_Name { get; set; }
    public string? Commodity_Name { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
}

public class CommodityLoad
{
    public int Id_Load { get; set; }
    public string? Scale_Ticket { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Vendor_Name { get; set; }
    public string? Truck { get; set; }
    public decimal? Gross_Weight { get; set; }
    public decimal? Tare_Weight { get; set; }
    public decimal? Net_Weight { get; set; }
    public DateTime? Date_In { get; set; }
}

public class CommoditySummary
{
    public string? Group_Name { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Dairy_Name { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
    public decimal? Avg_Weight { get; set; }
}

public class TicketResult
{
    public string? Scale_Ticket { get; set; }
    public bool Exists { get; set; }
}
