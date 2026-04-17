using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public interface ICrossCaptureService
{
    Task<(IEnumerable<CrossMilkLoad> Items, int TotalCount)> GetMilkLoadsAsync(
        int userId, int daysBefore, int skip, int take);

    Task<IEnumerable<CrossMilkLoadImage>> GetMilkLoadImagesAsync(int milkLoadId);

    Task<int> UpsertMilkLoadAsync(UpsertMilkLoadRequest request);

    Task<int> UpsertMilkLoadWithImagesAsync(UpsertMilkLoadWithImagesRequest request);

    Task UpdateMilkLoadImagesOthersAsync(UpdateMilkLoadImagesOthersRequest request);

    Task<(IEnumerable<CrossSiloStatusLoad> Items, int TotalCount)> GetSiloStatusLoadsAsync(
        int userId, int daysBefore, int skip, int take);

    Task<IEnumerable<CrossSiloStatusImage>> GetSiloStatusImagesAsync(int siloStatusLoadId);

    Task<int> UpsertSiloStatusWithImagesAsync(UpsertSiloStatusRequest request);
}
