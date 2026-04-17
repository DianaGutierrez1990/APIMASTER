using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.ScaleI;

[Route("api/v1/scalei")]
[Authorize(Policy = Policies.TenantReader)]
public class DeviceController : BaseController
{
    private readonly IScaleiService _scaleiService;

    public DeviceController(IScaleiService scaleiService)
    {
        _scaleiService = scaleiService;
    }

    /// <summary>POST /save-pulse — Scale device heartbeat</summary>
    [HttpPost("save-pulse")]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> SavePulse([FromBody] SavePulseRequest request)
    {
        await _scaleiService.SavePulseAsync(request);
        return Ok(new { status = "ok" });
    }

    /// <summary>POST /load-types — Get load type catalog</summary>
    [HttpPost("load-types")]
    public async Task<IActionResult> GetLoadTypes()
    {
        var types = await _scaleiService.GetLoadTypesAsync();
        return Ok(types);
    }
}
