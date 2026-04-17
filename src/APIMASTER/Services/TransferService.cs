using Dapper;
using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public class TransferService : ITransferService
{
    private readonly IDatabaseResolver _dbResolver;

    public TransferService(IDatabaseResolver dbResolver)
    {
        _dbResolver = dbResolver;
    }

    public async Task<(IEnumerable<TransferCommodity> Items, int TotalCount)> GetCommoditiesAsync(
        DateTime startDate, DateTime endDate, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Transfer_Commodities(@StartDate, @EndDate)
            ORDER BY Commodity_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Transfer_Commodities(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<TransferCommodity>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<TransferDairy> Items, int TotalCount)> GetDairiesAsync(
        DateTime startDate, DateTime endDate, int? commodityId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Transfer_Dairies(@StartDate, @EndDate, @CommodityId)
            ORDER BY Dairy_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Transfer_Dairies(@StartDate, @EndDate, @CommodityId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, CommodityId = commodityId ?? 0, Skip = skip, Take = take });

        var items = await multi.ReadAsync<TransferDairy>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<TransferLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int? commodityId, int? dairyId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Transfer_Loads(@StartDate, @EndDate, @CommodityId, @DairyId)
            ORDER BY Transfer_Date DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Transfer_Loads(@StartDate, @EndDate, @CommodityId, @DairyId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, CommodityId = commodityId ?? 0, DairyId = dairyId ?? 0, Skip = skip, Take = take });

        var items = await multi.ReadAsync<TransferLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<TransferSummary> Items, int TotalCount)> GetSummaryCommodityAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take)
    {
        var functionName = $"MV_List_Transfer_Summary_Commodity_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate)
            ORDER BY Group_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Skip = skip, Take = take });

        var items = await multi.ReadAsync<TransferSummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<TransferSummary> Items, int TotalCount)> GetSummaryRouteAsync(
        DateTime startDate, DateTime endDate, string group, int? commodityId, int skip, int take)
    {
        var functionName = $"MV_List_Transfer_Summary_Route_{group}";

        var sql = $@"
            SELECT * FROM {functionName}(@StartDate, @EndDate, @CommodityId)
            ORDER BY Group_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM {functionName}(@StartDate, @EndDate, @CommodityId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, CommodityId = commodityId ?? 0, Skip = skip, Take = take });

        var items = await multi.ReadAsync<TransferSummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }
}
