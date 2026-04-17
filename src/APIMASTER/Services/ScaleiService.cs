using Dapper;
using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public class ScaleiService : IScaleiService
{
    private readonly IDatabaseResolver _dbResolver;
    private readonly ILogger<ScaleiService> _logger;

    public ScaleiService(IDatabaseResolver dbResolver, ILogger<ScaleiService> logger)
    {
        _dbResolver = dbResolver;
        _logger = logger;
    }

    /// <summary>SI_List_Scale_Loads(@ID_Weight, @ID_Dairy, @ILoad, @StDate, @EdDate)</summary>
    public async Task<IEnumerable<ScaleLoad>> GetScaleLoadsAsync(ScaleLoadsRequest request)
    {
        var sql = "SELECT * FROM [dbo].[SI_List_Scale_Loads](@ID_Weight, @ID_Dairy, @ILoad, @StDate, @EdDate)";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        return await conn.QueryAsync<ScaleLoad>(sql, new
        {
            ID_Weight = (Guid?)null,
            ID_Dairy = string.IsNullOrEmpty(request.IdDairy) ? (Guid?)null : Guid.Parse(request.IdDairy),
            ILoad = request.InLoad,
            StDate = request.StartDate,
            EdDate = request.EndDate
        });
    }

    /// <summary>List_Loads_Capture(@ID_Weight)</summary>
    public async Task<IEnumerable<LoadCapture>> GetLoadCapturesAsync(string idWeight)
    {
        const string sql = "SELECT * FROM [dbo].[List_Loads_Capture](@ID_Weight)";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        return await conn.QueryAsync<LoadCapture>(sql, new { ID_Weight = idWeight });
    }

    /// <summary>List_Scale_Images(@StaDate, @EdDate)</summary>
    public async Task<IEnumerable<ScaleImageSummary>> GetScaleImagesAsync(string startDate, string endDate)
    {
        const string sql = "SELECT * FROM [dbo].[List_Scale_Images](@StaDate, @EdDate)";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        return await conn.QueryAsync<ScaleImageSummary>(sql, new
        {
            StaDate = startDate,
            EdDate = endDate
        });
    }

    /// <summary>List_Load_Type() — no params</summary>
    public async Task<IEnumerable<ScaleLoadType>> GetLoadTypesAsync()
    {
        const string sql = "SELECT * FROM [dbo].[List_Load_Type]()";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        return await conn.QueryAsync<ScaleLoadType>(sql);
    }

    /// <summary>List_Document_Type(@param) — General_Dairyi</summary>
    public async Task<IEnumerable<DocumentTypeItem>> GetDocumentTypesAsync()
    {
        const string sql = "SELECT * FROM [dbo].[List_Document_Type](@param)";

        using var conn = _dbResolver.GetConnectionByName("GeneralDairyi");
        await conn.OpenAsync();
        return await conn.QueryAsync<DocumentTypeItem>(sql, new { param = string.Empty });
    }

    /// <summary>Update_ITXScale_Images SP</summary>
    public async Task StoreImageAsync(string userId, StoreImageRequest request)
    {
        const string sql = @"EXEC [dbo].[Update_ITXScale_Images]
            @ID_Image, @ID_Capture, @ID_Weight, @ScaleTicket, @BOL, @Image, @Date, @ID_User, @Document_Type";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new
        {
            ID_Image = Guid.NewGuid(),
            ID_Capture = Guid.Parse(request.IdCapture),
            ID_Weight = Guid.Parse(request.IdWeight),
            ScaleTicket = request.ScaleTicket,
            BOL = request.Bol,
            Image = request.Image,
            Date = DateTime.UtcNow,
            ID_User = Guid.Parse(userId),
            Document_Type = request.DocumentType
        });

        _logger.LogInformation("Image stored for weight {IdWeight} by user {UserId}", request.IdWeight, userId);
    }

    /// <summary>Bulk version of Update_ITXScale_Images</summary>
    public async Task<StoreBulkResponse> StoreImagesBulkAsync(string userId, List<StoreImageItem> items)
    {
        const string sql = @"EXEC [dbo].[Update_ITXScale_Images]
            @ID_Image, @ID_Capture, @ID_Weight, @ScaleTicket, @BOL, @Image, @Date, @ID_User, @Document_Type";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();

        var results = new List<StoreBulkItemResult>(items.Count);
        var failedCount = 0;

        foreach (var item in items)
        {
            try
            {
                await conn.ExecuteAsync(sql, new
                {
                    ID_Image = Guid.NewGuid(),
                    ID_Capture = Guid.Parse(item.IdCapture),
                    ID_Weight = Guid.Parse(item.IdWeight),
                    ScaleTicket = item.ScaleTicket,
                    BOL = item.Bol,
                    Image = item.Image,
                    Date = DateTime.UtcNow,
                    ID_User = Guid.Parse(userId),
                    Document_Type = item.DocumentType
                });

                results.Add(new StoreBulkItemResult { IdWeight = item.IdWeight, Status = "ok" });
            }
            catch (Exception ex)
            {
                failedCount++;
                results.Add(new StoreBulkItemResult { IdWeight = item.IdWeight, Status = "error", Message = ex.Message });
                _logger.LogWarning(ex, "Failed to store image for weight {IdWeight}", item.IdWeight);
            }
        }

        return new StoreBulkResponse
        {
            Status = failedCount == 0 ? "ok" : "partial",
            Processed = items.Count - failedCount,
            Failed = failedCount,
            Results = results
        };
    }

    /// <summary>Update_ITXScale_Documents SP</summary>
    public async Task StoreDocumentAsync(string userId, StoreDocumentRequest request)
    {
        const string sql = @"EXEC [dbo].[Update_ITXScale_Documents]
            @ID_ITXScale_Documents, @ID_Weight, @Document_Name, @Document_Path, @Document_Ext, @Description, @ID_User";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new
        {
            ID_ITXScale_Documents = Guid.NewGuid(),
            ID_Weight = Guid.Parse(request.IdWeight),
            Document_Name = request.DocumentName,
            Document_Path = request.DocumentPath,
            Document_Ext = request.DocumentExt,
            Description = request.Description,
            ID_User = Guid.Parse(userId)
        });

        _logger.LogInformation("Document stored for weight {IdWeight} by user {UserId}", request.IdWeight, userId);
    }

    /// <summary>SI_Scale_Pulse SP</summary>
    public async Task SavePulseAsync(SavePulseRequest request)
    {
        const string sql = @"EXEC [dbo].[SI_Scale_Pulse]
            @ID_Scale, @PulseDate, @Mac, @PCName, @Version, @PendingChanges, @ScaleActive, @IP";

        using var conn = _dbResolver.GetConnectionByName("ICCManager");
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new
        {
            ID_Scale = Guid.Parse(request.IdScale),
            PulseDate = DateTime.Parse(request.PulseDate),
            Mac = request.Mac,
            PCName = request.PcName,
            Version = request.Version,
            PendingChanges = request.PendingChanges,
            ScaleActive = request.ScaleActive,
            IP = request.Ip
        });

        _logger.LogDebug("Pulse saved for scale {IdScale}", request.IdScale);
    }
}
