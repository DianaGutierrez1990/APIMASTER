namespace APIMASTER.Data.Entities;

public class CrossSiloStatusLoad
{
    public int Id_Silo_Status_Loads { get; set; }
    public int? Id_Silo { get; set; }
    public string? Silo_Name { get; set; }
    public string? Status { get; set; }
    public decimal? Current_Weight { get; set; }
    public decimal? Capacity { get; set; }
    public decimal? Temperature { get; set; }
    public string? Notes { get; set; }
    public int? Id_User { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Updated_At { get; set; }
}

public class CrossSiloStatusImage
{
    public int Id_Silo_Status_Loads_Image { get; set; }
    public int Id_Silo_Status_Loads { get; set; }
    public string? Image_Name { get; set; }
    public string? Image_Type { get; set; }
    public string? Image_Path { get; set; }
    public DateTime? Created_At { get; set; }
}
