namespace APIMASTER.Data.Entities;

public class TransferCommodity
{
    public int Id_Commodity { get; set; }
    public string? Commodity_Name { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
}

public class TransferDairy
{
    public int Id_Dairy { get; set; }
    public string? Dairy_Name { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
}

public class TransferLoad
{
    public int Id_Load { get; set; }
    public string? Scale_Ticket { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public string? Truck { get; set; }
    public decimal? Net_Weight { get; set; }
    public DateTime? Transfer_Date { get; set; }
}

public class TransferSummary
{
    public string? Group_Name { get; set; }
    public string? Commodity_Name { get; set; }
    public string? Route { get; set; }
    public decimal? Total_Weight { get; set; }
    public int? Total_Loads { get; set; }
    public decimal? Avg_Weight { get; set; }
}
