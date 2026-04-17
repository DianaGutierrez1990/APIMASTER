using Dapper;
using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public class HarvestService : IHarvestService
{
    private readonly IDatabaseResolver _dbResolver;

    public HarvestService(IDatabaseResolver dbResolver)
    {
        _dbResolver = dbResolver;
    }

    public async Task<(IEnumerable<Harvest> Items, int TotalCount)> GetHarvestsAsync(
        DateTime startDate, DateTime endDate, int? fieldId, string? crop, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Harvest(@StartDate, @EndDate, @FieldId, @Crop)
            ORDER BY Harvest_Date DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Harvest(@StartDate, @EndDate, @FieldId, @Crop);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, FieldId = fieldId ?? 0, Crop = crop ?? "", Skip = skip, Take = take });

        var items = await multi.ReadAsync<Harvest>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<FieldSummary> Items, int TotalCount)> GetFieldSummaryAsync(
        DateTime startDate, DateTime endDate, string? crop, string? field, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Field_Summary(@StartDate, @EndDate, @Crop, @Field)
            ORDER BY Field_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Field_Summary(@StartDate, @EndDate, @Crop, @Field);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Crop = crop ?? "", Field = field ?? "", Skip = skip, Take = take });

        var items = await multi.ReadAsync<FieldSummary>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<IEnumerable<SummaryFilter>> GetSummaryFiltersAsync()
    {
        const string sql = "SELECT * FROM MV_List_Summary_Filters()";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        return await conn.QueryAsync<SummaryFilter>(sql);
    }

    public async Task<(IEnumerable<FieldDryMatter> Items, int TotalCount)> GetFieldDryMatterAsync(
        DateTime startDate, DateTime endDate, string? field, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Field_DryMatter(@StartDate, @EndDate, @Field)
            ORDER BY Sample_Date DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Field_DryMatter(@StartDate, @EndDate, @Field);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, Field = field ?? "", Skip = skip, Take = take });

        var items = await multi.ReadAsync<FieldDryMatter>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<FieldLoad> Items, int TotalCount)> GetFieldLoadsAsync(
        DateTime startDate, DateTime endDate, int? fieldId, int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Field_Loads(@StartDate, @EndDate, @FieldId)
            ORDER BY Load_Date DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Field_Loads(@StartDate, @EndDate, @FieldId);";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { StartDate = startDate, EndDate = endDate, FieldId = fieldId ?? 0, Skip = skip, Take = take });

        var items = await multi.ReadAsync<FieldLoad>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<Truck> Items, int TotalCount)> GetTrucksAsync(int skip, int take)
    {
        const string sql = @"
            SELECT * FROM MV_List_Trucks()
            ORDER BY Truck_Name ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM MV_List_Trucks();";

        using var conn = _dbResolver.GetConnection("dairy");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { Skip = skip, Take = take });

        var items = await multi.ReadAsync<Truck>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }
}
