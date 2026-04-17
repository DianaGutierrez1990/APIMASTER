using Dapper;
using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public class CrossCaptureService : ICrossCaptureService
{
    private readonly IDatabaseResolver _dbResolver;
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<CrossCaptureService> _logger;

    public CrossCaptureService(
        IDatabaseResolver dbResolver,
        IFileStorageService fileStorage,
        ILogger<CrossCaptureService> logger)
    {
        _dbResolver = dbResolver;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<(IEnumerable<CrossMilkLoad> Items, int TotalCount)> GetMilkLoadsAsync(
        int userId, int daysBefore, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_User_Milk_Load_List(@UserId, @DaysBefore, @CurrentDate)
            ORDER BY Date_In DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_User_Milk_Load_List(@UserId, @DaysBefore, @CurrentDate);";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new
        {
            UserId = userId,
            DaysBefore = daysBefore,
            CurrentDate = DateTime.UtcNow,
            Skip = skip,
            Take = take
        });

        var items = await multi.ReadAsync<CrossMilkLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<IEnumerable<CrossMilkLoadImage>> GetMilkLoadImagesAsync(int milkLoadId)
    {
        const string sql = "SELECT * FROM MV_Milk_Load_Images(@MilkLoadId)";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossMilkLoadImage>(sql, new { MilkLoadId = milkLoadId });
    }

    public async Task<int> UpsertMilkLoadAsync(UpsertMilkLoadRequest request)
    {
        // 1. Save image if provided
        string? imageName = null;
        if (request.Image != null && !string.IsNullOrEmpty(request.ImageType))
        {
            imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
            await _fileStorage.SaveFileAsync(
                request.Image, "cross_manifest", request.ImageType, imageName);
        }

        // 2. Execute Stored Procedure
        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();

        var milkLoadId = await conn.ExecuteScalarAsync<int>(
            "[Cross].[dbo].[Update_Milk_Load_2]",
            new
            {
                id_milk_load = request.IdMilkLoad,
                id_customer_dairy_barn_silo = request.IdCustomerDairyBarnSilo,
                scale_ticket = request.ScaleTicket,
                manifest = request.Manifest,
                truck = request.Truck,
                trailer = request.Trailer,
                driver = request.Driver,
                seal = request.Seal,
                gross_weight = request.GrossWeight,
                tare_weight = request.TareWeight,
                net_weight = request.NetWeight,
                temperature = request.Temperature,
                date_in = request.DateIn,
                date_out = request.DateOut,
                status = request.Status,
                notes = request.Notes,
                id_user = request.IdUser,
                image_name = imageName,
                image_type = request.ImageType
            },
            commandType: System.Data.CommandType.StoredProcedure);

        // 3. Register image in DB if saved
        if (!string.IsNullOrEmpty(imageName))
        {
            await conn.ExecuteAsync(
                "[Cross].[dbo].[Update_Milk_Load_Images2]",
                new
                {
                    id_milk_load = milkLoadId,
                    image_name = imageName,
                    image_type = request.ImageType
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        _logger.LogInformation("Upserted milk load {MilkLoadId} by user {UserId}", milkLoadId, request.IdUser);
        return milkLoadId;
    }

    public async Task<int> UpsertMilkLoadWithImagesAsync(UpsertMilkLoadWithImagesRequest request)
    {
        // 1. Execute SP to upsert load
        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();

        var milkLoadId = await conn.ExecuteScalarAsync<int>(
            "[Cross].[dbo].[Update_Milk_Load_2]",
            new
            {
                id_milk_load = request.IdMilkLoad,
                id_customer_dairy_barn_silo = request.IdCustomerDairyBarnSilo,
                scale_ticket = request.ScaleTicket,
                manifest = request.Manifest,
                truck = request.Truck,
                trailer = request.Trailer,
                driver = request.Driver,
                seal = request.Seal,
                gross_weight = request.GrossWeight,
                tare_weight = request.TareWeight,
                net_weight = request.NetWeight,
                temperature = request.Temperature,
                date_in = request.DateIn,
                date_out = request.DateOut,
                status = request.Status,
                notes = request.Notes,
                id_user = request.IdUser,
                image_name = (string?)null,
                image_type = (string?)null
            },
            commandType: System.Data.CommandType.StoredProcedure);

        // 2. Save each image
        var imageTypes = (request.ImageTypes ?? "").Split(',', StringSplitOptions.TrimEntries);

        for (var i = 0; i < request.Images.Count; i++)
        {
            var file = request.Images[i];
            var imageType = i < imageTypes.Length ? imageTypes[i] : "others";
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _fileStorage.SaveFileAsync(file, "cross_manifest", imageType, imageName);

            await conn.ExecuteAsync(
                "[Cross].[dbo].[Update_Milk_Load_Images2]",
                new
                {
                    id_milk_load = milkLoadId,
                    image_name = imageName,
                    image_type = imageType
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        _logger.LogInformation("Upserted milk load {MilkLoadId} with {ImageCount} images", milkLoadId, request.Images.Count);
        return milkLoadId;
    }

    public async Task UpdateMilkLoadImagesOthersAsync(UpdateMilkLoadImagesOthersRequest request)
    {
        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();

        foreach (var file in request.Images)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _fileStorage.SaveFileAsync(file, "cross_manifest", "others", imageName);

            await conn.ExecuteAsync(
                "[Cross].[dbo].[Update_Milk_Load_Images2]",
                new
                {
                    id_milk_load = request.IdMilkLoad,
                    image_name = imageName,
                    image_type = "others"
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        _logger.LogInformation("Updated {Count} 'others' images for milk load {MilkLoadId}",
            request.Images.Count, request.IdMilkLoad);
    }

    public async Task<(IEnumerable<CrossSiloStatusLoad> Items, int TotalCount)> GetSiloStatusLoadsAsync(
        int userId, int daysBefore, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_User_Silo_Status_Loads_List(@UserId, @DaysBefore, @CurrentDate)
            ORDER BY Created_At DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_User_Silo_Status_Loads_List(@UserId, @DaysBefore, @CurrentDate);";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new
        {
            UserId = userId,
            DaysBefore = daysBefore,
            CurrentDate = DateTime.UtcNow,
            Skip = skip,
            Take = take
        });

        var items = await multi.ReadAsync<CrossSiloStatusLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<IEnumerable<CrossSiloStatusImage>> GetSiloStatusImagesAsync(int siloStatusLoadId)
    {
        const string sql = "SELECT * FROM MV_Silo_Status_Loads_Images(@SiloStatusLoadId)";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossSiloStatusImage>(sql, new { SiloStatusLoadId = siloStatusLoadId });
    }

    public async Task<int> UpsertSiloStatusWithImagesAsync(UpsertSiloStatusRequest request)
    {
        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();

        var siloStatusId = await conn.ExecuteScalarAsync<int>(
            "[Cross].[dbo].[Update_Silo_Status_Loads]",
            new
            {
                id_silo_status_loads = request.IdSiloStatusLoads,
                id_silo = request.IdSilo,
                status = request.Status,
                current_weight = request.CurrentWeight,
                capacity = request.Capacity,
                temperature = request.Temperature,
                notes = request.Notes,
                id_user = request.IdUser
            },
            commandType: System.Data.CommandType.StoredProcedure);

        // Save images
        var imageTypes = (request.ImageTypes ?? "").Split(',', StringSplitOptions.TrimEntries);

        for (var i = 0; i < request.Images.Count; i++)
        {
            var file = request.Images[i];
            var imageType = i < imageTypes.Length ? imageTypes[i] : "others";
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _fileStorage.SaveFileAsync(file, "scalei", imageType, imageName);

            await conn.ExecuteAsync(
                "[Cross].[dbo].[Update_Silo_Status_Loads_Images]",
                new
                {
                    id_silo_status_loads = siloStatusId,
                    image_name = imageName,
                    image_type = imageType
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        _logger.LogInformation("Upserted silo status {SiloStatusId} with {ImageCount} images",
            siloStatusId, request.Images.Count);
        return siloStatusId;
    }
}
