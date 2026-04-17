using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public interface ICommoditiesService
{
    Task<(IEnumerable<CommodityPickup> Items, int TotalCount)> GetPickupAsync(
        DateTime startDate, DateTime endDate, int skip, int take);

    Task<(IEnumerable<CommodityDelivery> Items, int TotalCount)> GetDeliveryAsync(
        DateTime startDate, DateTime endDate, int skip, int take);

    Task<(IEnumerable<CommodityVendor> Items, int TotalCount)> GetVendorsAsync(
        DateTime startDate, DateTime endDate, int skip, int take);

    Task<(IEnumerable<CommodityLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int commodityId, int? vendorId, string? deliveryLocation, int skip, int take);

    Task<TicketResult> FindTicketAsync(string scaleTicket);

    Task<(IEnumerable<CommoditySummary> Items, int TotalCount)> GetSummaryAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take);

    Task<(IEnumerable<CommoditySummary> Items, int TotalCount)> GetSummaryByDairyAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take);
}
