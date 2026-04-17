using Dapper;
using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public class MilkService : IMilkService
{
    private readonly IDatabaseResolver _dbResolver;
    private readonly ILogger<MilkService> _logger;

    public MilkService(IDatabaseResolver dbResolver, ILogger<MilkService> logger)
    {
        _dbResolver = dbResolver;
        _logger = logger;
    }

    public async Task<(IEnumerable<MilkCustomer> Items, int TotalCount)> GetCustomersAsync(
        DateTime startDate, DateTime endDate, int locationId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Milk_Customers(@StartDate, @EndDate, @LocationId)
            ORDER BY Delivery_Location ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Milk_Customers(@StartDate, @EndDate, @LocationId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, LocationId = locationId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkCustomer>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkDairy> Items, int TotalCount)> GetDairiesAsync(
        DateTime startDate, DateTime endDate, int locationId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Milk_Dairy(@StartDate, @EndDate, @LocationId)
            ORDER BY Dairy ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Milk_Dairy(@StartDate, @EndDate, @LocationId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, LocationId = locationId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkDairy>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int dairyId, int customerLocationId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Milk_Loads(@StartDate, @EndDate, @DairyId, @CustomerLocationId)
            ORDER BY DateIn DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Milk_Loads(@StartDate, @EndDate, @DairyId, @CustomerLocationId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, DairyId = dairyId, CustomerLocationId = customerLocationId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkLoad> Items, int TotalCount)> SearchLoadsAsync(
        string? ticket, string? manifest, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Milk_Loads_Search(@Ticket, @Manifest)
            ORDER BY DateIn DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Milk_Loads_Search(@Ticket, @Manifest);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { Ticket = ticket ?? "", Manifest = manifest ?? "", Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkSummary> Items, int TotalCount)> GetSummaryByCustomerAsync(
        DateTime startDate, DateTime endDate, int dairyId, string deliveryLocation, string group, int skip, int take)
    {
        var functionName = $"MV_List_Milk_Summary_By_Customer_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate, @DairyId, @DeliveryLocation)
            ORDER BY Customer ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate, @DairyId, @DeliveryLocation);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, DairyId = dairyId, DeliveryLocation = deliveryLocation, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkSummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkSummary> Items, int TotalCount)> GetSummaryByCustomerDairyAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take)
    {
        var functionName = $"MV_List_Milk_Summary_By_Customer_Dairy_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate, '', '')
            ORDER BY Customer ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate, '', '');";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkSummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<MilkDeliveryStatus> Items, int TotalCount)> GetDeliveryMonthlyStatusAsync(
        DateTime startDate, DateTime endDate, int customerId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Milk_Delivery_Loads_Monthly_Status(@StartDate, @EndDate, @CustomerId)
            ORDER BY Year ASC, Month ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Milk_Delivery_Loads_Monthly_Status(@StartDate, @EndDate, @CustomerId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, CustomerId = customerId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<MilkDeliveryStatus>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<IEnumerable<MilkLoadImage>> GetImagesAsync(int milkLoadId)
    {
        const string sql = "SELECT * FROM MV_Milk_Load_Images(@MilkLoadId)";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<MilkLoadImage>(sql, new { MilkLoadId = milkLoadId });
    }
}
