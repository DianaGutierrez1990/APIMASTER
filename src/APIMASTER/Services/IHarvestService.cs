using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public interface IHarvestService
{
    Task<(IEnumerable<Harvest> Items, int TotalCount)> GetHarvestsAsync(
        DateTime startDate, DateTime endDate, int? fieldId, string? crop, int skip, int take);

    Task<(IEnumerable<FieldSummary> Items, int TotalCount)> GetFieldSummaryAsync(
        DateTime startDate, DateTime endDate, string? crop, string? field, int skip, int take);

    Task<IEnumerable<SummaryFilter>> GetSummaryFiltersAsync();

    Task<(IEnumerable<FieldDryMatter> Items, int TotalCount)> GetFieldDryMatterAsync(
        DateTime startDate, DateTime endDate, string? field, int skip, int take);

    Task<(IEnumerable<FieldLoad> Items, int TotalCount)> GetFieldLoadsAsync(
        DateTime startDate, DateTime endDate, int? fieldId, int skip, int take);

    Task<(IEnumerable<Truck> Items, int TotalCount)> GetTrucksAsync(int skip, int take);
}
