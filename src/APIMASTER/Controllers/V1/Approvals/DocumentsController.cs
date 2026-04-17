using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.Approvals;

[Route("api/v1/approvals")]
[Authorize(Policy = Policies.TenantReader)]
public class DocumentsController : BaseController
{
    private readonly IApprovalsService _approvalsService;

    public DocumentsController(IApprovalsService approvalsService)
    {
        _approvalsService = approvalsService;
    }

    [HttpGet("managers")]
    public async Task<IActionResult> GetManagers()
    {
        var managers = await _approvalsService.GetManagersAsync();

        if (!managers.Any())
            return NotFoundResponse("No managers found.");

        return OkResponse(managers);
    }

    [HttpPost("documents")]
    [Authorize(Policy = Policies.TenantWriter)]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(25 * 1024 * 1024)]
    public async Task<IActionResult> SaveDocument([FromForm] SaveDocumentRequest request)
    {
        var docId = await _approvalsService.SaveDocumentAsync(request);
        return Created(string.Empty, new { id = docId, requestId = RequestId });
    }
}
