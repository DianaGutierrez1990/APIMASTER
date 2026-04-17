using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public interface ITransferService
{
    Task<(IEnumerable<TransferCommodity> Items, int TotalCount)> GetCommoditiesAsync(
        DateTime startDate, DateTime endDate, int skip, int take);

    Task<(IEnumerable<TransferDairy> Items, int TotalCount)> GetDairiesAsync(
        DateTime startDate, DateTime endDate, int? commodityId, int skip, int take);

    Task<(IEnumerable<TransferLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int? commodityId, int? dairyId, int skip, int take);

    Task<(IEnumerable<TransferSummary> Items, int TotalCount)> GetSummaryCommodityAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take);

    Task<(IEnumerable<TransferSummary> Items, int TotalCount)> GetSummaryRouteAsync(
        DateTime startDate, DateTime endDate, string group, int? commodityId, int skip, int take);
}
