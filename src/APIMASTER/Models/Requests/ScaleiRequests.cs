using System.Text.Json.Serialization;

namespace APIMASTER.Models.Requests;

// ── POST /scale-loads — list scale loads ──
public class ScaleLoadsRequest
{
    [JsonPropertyName("id_dairy")]
    public string? IdDairy { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;

    [JsonPropertyName("in_load")]
    public bool? InLoad { get; set; }
}

// ── POST /captures — list captures for a weight ──
public class LoadCapturesRequest
{
    [JsonPropertyName("id_weight")]
    public string IdWeight { get; set; } = string.Empty;
}

// ── POST /scale-images — summary of images by dairy/date ──
public class ScaleImagesRequest
{
    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;
}

// ── POST /store-image — save a single evidence into General_Dairyi.App_Loads_capture ──
public class StoreImageRequest
{
    [JsonPropertyName("id_capture")]
    public string IdCapture { get; set; } = string.Empty;

    [JsonPropertyName("scale_ticket")]
    public string ScaleTicket { get; set; } = string.Empty;

    [JsonPropertyName("bol")]
    public string Bol { get; set; } = string.Empty;

    [JsonPropertyName("id_user")]
    public string? IdUser { get; set; }

    [JsonPropertyName("image_name")]
    public string ImageName { get; set; } = string.Empty;

    [JsonPropertyName("image_route")]
    public string ImageRoute { get; set; } = string.Empty;

    [JsonPropertyName("document_type")]
    public string? DocumentType { get; set; }
}

// ── POST /store-images-bulk — save up to 10 evidences ──
public class StoreImagesBulkRequest
{
    [JsonPropertyName("items")]
    public List<StoreImageItem> Items { get; set; } = [];
}

public class StoreImageItem
{
    [JsonPropertyName("id_capture")]
    public string IdCapture { get; set; } = string.Empty;

    [JsonPropertyName("scale_ticket")]
    public string ScaleTicket { get; set; } = string.Empty;

    [JsonPropertyName("bol")]
    public string Bol { get; set; } = string.Empty;

    [JsonPropertyName("id_user")]
    public string? IdUser { get; set; }

    [JsonPropertyName("image_name")]
    public string ImageName { get; set; } = string.Empty;

    [JsonPropertyName("image_route")]
    public string ImageRoute { get; set; } = string.Empty;

    [JsonPropertyName("document_type")]
    public string? DocumentType { get; set; }
}

// ── POST /store-document — save a document ──
public class StoreDocumentRequest
{
    [JsonPropertyName("id_weight")]
    public string IdWeight { get; set; } = string.Empty;

    [JsonPropertyName("document_name")]
    public string DocumentName { get; set; } = string.Empty;

    [JsonPropertyName("document_path")]
    public string DocumentPath { get; set; } = string.Empty;

    [JsonPropertyName("document_ext")]
    public string DocumentExt { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

// ── POST /save-pulse ──
public class SavePulseRequest
{
    [JsonPropertyName("id_scale")]
    public string IdScale { get; set; } = string.Empty;

    [JsonPropertyName("pulse_date")]
    public string PulseDate { get; set; } = string.Empty;

    [JsonPropertyName("mac")]
    public string Mac { get; set; } = string.Empty;

    [JsonPropertyName("pc_name")]
    public string PcName { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("pending_changes")]
    public string PendingChanges { get; set; } = string.Empty;

    [JsonPropertyName("scale_active")]
    public bool ScaleActive { get; set; }

    [JsonPropertyName("ip")]
    public string Ip { get; set; } = string.Empty;
}
