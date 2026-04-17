namespace APIMASTER.Data.Entities;

public class CrossMilkLoadImage
{
    public int Id_Milk_Load_Image { get; set; }
    public int Id_Milk_Load { get; set; }
    public string? Image_Name { get; set; }
    public string? Image_Type { get; set; }
    public string? Image_Path { get; set; }
    public DateTime? Created_At { get; set; }
}
