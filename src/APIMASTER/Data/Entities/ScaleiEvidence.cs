using System.Text.Json.Serialization;

namespace APIMASTER.Data.Entities;

/// <summary>Row from SI_List_Scale_Loads TVF — 45 columns</summary>
public class ScaleLoad
{
    public Guid ID_Weight { get; set; }
    public Guid ID_Commodity { get; set; }
    public Guid ID_Location { get; set; }
    public Guid ID_Truck { get; set; }
    public Guid ID_Hauler { get; set; }
    public Guid ID_DairyLocation { get; set; }
    public Guid ID_Scale { get; set; }
    public Guid ID_LoadType { get; set; }
    public string? Load_Type { get; set; }
    public string? Commodity { get; set; }
    public string? Scale { get; set; }
    public string? Dairy { get; set; }
    public string? Location { get; set; }
    public string? Location_Type { get; set; }
    public string? Hauler { get; set; }
    public string? TruckNo { get; set; }
    public string? Harvest { get; set; }
    public int ScaleTicket_No { get; set; }
    public string? ScaleTicket { get; set; }
    public string? ScaleTicket_Code { get; set; }
    public string? DeliveryTicket { get; set; }
    public string? ScaleComments { get; set; }
    public decimal WeightIn { get; set; }
    public decimal WeightOut { get; set; }
    public decimal NW { get; set; }
    public decimal NWTon { get; set; }
    public DateTime? DateIn { get; set; }
    public DateTime? DateOut { get; set; }
    public int Qty { get; set; }
    public string? BOL { get; set; }
    public string? Manifest { get; set; }
    public string? Comments { get; set; }
    public string? Driver { get; set; }
    public string? Trailer { get; set; }
    public string? StatusDetail { get; set; }
    public int TimeIn { get; set; }
    public string? TimeIn_String { get; set; }
    public bool IsHarvest { get; set; }
    public decimal VisibleWeight { get; set; }
    public decimal LW { get; set; }
    public DateTime? LWDate { get; set; }
    public string? LWLocation { get; set; }
    public string? UnitsType { get; set; }
    public decimal Commodity_Temp { get; set; }
    public bool ManualUpdate { get; set; }
}

/// <summary>Row from List_Loads_Capture TVF — 7 columns</summary>
public class LoadCapture
{
    public Guid ID_Image { get; set; }
    public Guid ID_Capture { get; set; }
    public Guid ID_Weight { get; set; }
    public string? BOL { get; set; }
    public DateTime? Date { get; set; }
    public string? FilePath { get; set; }
    public string? Document_Type { get; set; }
}

/// <summary>Row from List_Scale_Images TVF — 5 columns</summary>
public class ScaleImageSummary
{
    public string? Dairy { get; set; }
    public DateTime? Date { get; set; }
    public int Count_Images { get; set; }
    public int Count_Loads { get; set; }
    public int Progress { get; set; }
}

/// <summary>Row from List_Load_Type TVF — 2 columns</summary>
public class ScaleLoadType
{
    public Guid ID_Load_Type { get; set; }
    public string? LoadType { get; set; }
}

/// <summary>Row from List_Document_Type TVF — General_Dairyi</summary>
public class DocumentTypeItem
{
    public Guid ID_Document_Type { get; set; }
    public string? DocumentType { get; set; }
}

// ── Store-image response models ──
public class StoreBulkResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("processed")]
    public int Processed { get; set; }

    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    [JsonPropertyName("results")]
    public List<StoreBulkItemResult> Results { get; set; } = [];
}

public class StoreBulkItemResult
{
    [JsonPropertyName("id_weight")]
    public string IdWeight { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
