namespace APIMASTER.Models.Requests;

public class UpsertMilkLoadWithImagesRequest
{
    public int IdMilkLoad { get; set; }
    public int IdCustomerDairyBarnSilo { get; set; }
    public string? ScaleTicket { get; set; }
    public string? Manifest { get; set; }
    public string? Truck { get; set; }
    public string? Trailer { get; set; }
    public string? Driver { get; set; }
    public string? Seal { get; set; }
    public decimal GrossWeight { get; set; }
    public decimal TareWeight { get; set; }
    public decimal NetWeight { get; set; }
    public decimal Temperature { get; set; }
    public DateTime? DateIn { get; set; }
    public DateTime? DateOut { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public int IdUser { get; set; }

    /// <summary>
    /// Comma-separated image types matching each file: "manifest,washtag,thermometer"
    /// </summary>
    public string? ImageTypes { get; set; }

    public List<IFormFile> Images { get; set; } = [];
}
