namespace APIMASTER.Models.Requests;

public class UpsertSiloStatusRequest
{
    public int IdSiloStatusLoads { get; set; }
    public int IdSilo { get; set; }
    public string? Status { get; set; }
    public decimal CurrentWeight { get; set; }
    public decimal Capacity { get; set; }
    public decimal Temperature { get; set; }
    public string? Notes { get; set; }
    public int IdUser { get; set; }

    /// <summary>
    /// Comma-separated image types matching each file
    /// </summary>
    public string? ImageTypes { get; set; }

    public List<IFormFile> Images { get; set; } = [];
}
