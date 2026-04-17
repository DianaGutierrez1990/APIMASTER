using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public interface IScaleiService
{
    Task<IEnumerable<ScaleLoad>> GetScaleLoadsAsync(ScaleLoadsRequest request);
    Task<IEnumerable<LoadCapture>> GetLoadCapturesAsync(string idWeight);
    Task<IEnumerable<ScaleImageSummary>> GetScaleImagesAsync(string startDate, string endDate);
    Task<IEnumerable<ScaleLoadType>> GetLoadTypesAsync();
    Task<IEnumerable<DocumentTypeItem>> GetDocumentTypesAsync();
    Task StoreImageAsync(string userId, StoreImageRequest request);
    Task<StoreBulkResponse> StoreImagesBulkAsync(string userId, List<StoreImageItem> items);
    Task StoreDocumentAsync(string userId, StoreDocumentRequest request);
    Task SavePulseAsync(SavePulseRequest request);
}
