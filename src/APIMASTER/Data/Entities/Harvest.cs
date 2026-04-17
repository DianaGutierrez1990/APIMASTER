namespace APIMASTER.Data.Entities;

public class Harvest
{
    public int Id_Harvest { get; set; }
    public string? Field_Name { get; set; }
    public string? Crop { get; set; }
    public string? Variety { get; set; }
    public decimal? Acres { get; set; }
    public decimal? Tons { get; set; }
    public decimal? Tons_Per_Acre { get; set; }
    public decimal? Dry_Matter { get; set; }
    public DateTime? Harvest_Date { get; set; }
    public string? Status { get; set; }
}

public class FieldSummary
{
    public string? Field_Name { get; set; }
    public string? Crop { get; set; }
    public decimal? Total_Acres { get; set; }
    public decimal? Total_Tons { get; set; }
    public decimal? Avg_Tons_Per_Acre { get; set; }
    public decimal? Avg_Dry_Matter { get; set; }
    public int? Total_Loads { get; set; }
}

public class SummaryFilter
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
}

public class FieldDryMatter
{
    public string? Field_Name { get; set; }
    public decimal? Dry_Matter { get; set; }
    public DateTime? Sample_Date { get; set; }
}

public class FieldLoad
{
    public int Id_Load { get; set; }
    public string? Field_Name { get; set; }
    public string? Truck { get; set; }
    public decimal? Tons { get; set; }
    public DateTime? Load_Date { get; set; }
}

public class Truck
{
    public int Id_Truck { get; set; }
    public string? Truck_Number { get; set; }
    public string? Truck_Name { get; set; }
    public string? Status { get; set; }
}
