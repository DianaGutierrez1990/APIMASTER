using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.Approvals;

[Route("api/v1/approvals/questions")]
[Authorize(Policy = Policies.TenantReader)]
public class QuestionsController : BaseController
{
    private readonly IApprovalsService _approvalsService;

    public QuestionsController(IApprovalsService approvalsService)
    {
        _approvalsService = approvalsService;
    }

    [HttpPost]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> StoreQuestion([FromBody] StoreQuestionRequest request)
    {
        var questionId = await _approvalsService.StoreQuestionAsync(request);
        return Created(string.Empty, new { id = questionId, requestId = RequestId });
    }

    [HttpGet("asked")]
    public async Task<IActionResult> GetAsked([FromQuery] QuestionsListRequest request)
    {
        var (items, totalCount) = await _approvalsService.GetAskedQuestionsAsync(
            request.UserId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No questions found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending([FromQuery] QuestionsListRequest request)
    {
        var (items, totalCount) = await _approvalsService.GetPendingQuestionsAsync(
            request.UserId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No pending questions found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpPut("{questionId:int}/status")]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> ChangeStatus(int questionId, [FromBody] ChangeStatusRequest request)
    {
        request.IdQuestion = questionId;
        await _approvalsService.ChangeStatusAsync(request);
        return Ok(new { message = "Status updated.", requestId = RequestId });
    }
}
