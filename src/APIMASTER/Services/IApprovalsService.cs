using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public interface IApprovalsService
{
    Task<IEnumerable<Manager>> GetManagersAsync();
    Task<int> SaveDocumentAsync(SaveDocumentRequest request);
    Task<int> StoreQuestionAsync(StoreQuestionRequest request);
    Task<(IEnumerable<ApprovalQuestion> Items, int TotalCount)> GetAskedQuestionsAsync(int userId, int skip, int take);
    Task<(IEnumerable<ApprovalQuestion> Items, int TotalCount)> GetPendingQuestionsAsync(int userId, int skip, int take);
    Task ChangeStatusAsync(ChangeStatusRequest request);
}
