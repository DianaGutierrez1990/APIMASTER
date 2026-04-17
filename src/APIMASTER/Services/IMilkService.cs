using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public interface IMilkService
{
    Task<(IEnumerable<MilkCustomer> Items, int TotalCount)> GetCustomersAsync(
        DateTime startDate, DateTime endDate, int locationId, int skip, int take);

    Task<(IEnumerable<MilkDairy> Items, int TotalCount)> GetDairiesAsync(
        DateTime startDate, DateTime endDate, int locationId, int skip, int take);

    Task<(IEnumerable<MilkLoad> Items, int TotalCount)> GetLoadsAsync(
        DateTime startDate, DateTime endDate, int dairyId, int customerLocationId, int skip, int take);

    Task<(IEnumerable<MilkLoad> Items, int TotalCount)> SearchLoadsAsync(
        string? ticket, string? manifest, int skip, int take);

    Task<(IEnumerable<MilkSummary> Items, int TotalCount)> GetSummaryByCustomerAsync(
        DateTime startDate, DateTime endDate, int dairyId, string deliveryLocation, string group, int skip, int take);

    Task<(IEnumerable<MilkSummary> Items, int TotalCount)> GetSummaryByCustomerDairyAsync(
        DateTime startDate, DateTime endDate, string group, int skip, int take);

    Task<(IEnumerable<MilkDeliveryStatus> Items, int TotalCount)> GetDeliveryMonthlyStatusAsync(
        DateTime startDate, DateTime endDate, int customerId, int skip, int take);

    Task<IEnumerable<MilkLoadImage>> GetImagesAsync(int milkLoadId);
}
