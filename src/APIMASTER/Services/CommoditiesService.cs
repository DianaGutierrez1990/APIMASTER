using Dapper;
using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public class CommoditiesService : ICommoditiesService
{
    private readonly IDatabaseResolver _dbResolver;

    public CommoditiesService(IDatabaseResolver dbResolver)
    {
        _dbResolver = dbResolver;
    }

    public async Task<(IEnumerable<CommodityPickup> Items, int TotalCount)> GetPickupAsync(
        DateTime startDate, DateTime endDate, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Commodities_Pickup(@StartDate, @EndDate)
            ORDER BY Commodity_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Commodities_Pickup(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommodityPickup>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<CommodityDelivery> Items, int TotalCount)> GetDeliveryAsync(
        DateTime startDate, DateTime endDate, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Commodities_Delivery(@StartDate, @EndDate)
            ORDER BY Commodity_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Commodities_Delivery(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommodityDelivery>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<CommodityVendor> Items, int TotalCount)> GetVendorsAsync(
        DateTime startDate, DateTime endDate, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Commodities_Vendor(@StartDate, @EndDate)
            ORDER BY Vendor_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Commodities_Vendor(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommodityVendor>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<CommodityLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int commodityId, int? vendorId, string? deliveryLocation, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Commodities_Loads(@StartDate, @EndDate, @CommodityId, @VendorId, @DeliveryLocation)
            ORDER BY Date_In DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Commodities_Loads(@StartDate, @EndDate, @CommodityId, @VendorId, @DeliveryLocation);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, CommodityId = commodityId, VendorId = vendorId ?? 0, DeliveryLocation = deliveryLocation ?? "", Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommodityLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<TicketResult> FindTicketAsync(string scaleTicket)
    {
        const string sql = "SELECT * FROM check_ticket_exists(@ScaleTicket)";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        var exists = await conn.ExecuteScalarAsync<int>(sql, new { ScaleTicket = scaleTicket });
        return new TicketResult { Scale_Ticket = scaleTicket, Exists = exists > 0 };
    }

    public async Task<(IEnumerable<CommoditySummary> Items, int TotalCount)> GetSummaryAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take)
    {
        var functionName = $"MV_List_Commodities_Summary_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate)
            ORDER BY Group_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommoditySummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<CommoditySummary> Items, int TotalCount)> GetSummaryByDairyAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take)
    {
        var functionName = $"MV_List_Commodities_Summary_Dairy_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate)
            ORDER BY Group_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<CommoditySummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }
}
