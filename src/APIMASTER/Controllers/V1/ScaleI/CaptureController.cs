using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.ScaleI;

[Route("api/v1/scalei")]
[Authorize(Policy = Policies.TenantReader)]
public class CaptureController : BaseController
{
    private readonly IScaleiService _scaleiService;

    public CaptureController(IScaleiService scaleiService)
    {
        _scaleiService = scaleiService;
    }

    /// <summary>POST /scale-loads — List scale loads by dairy/date range</summary>
    [HttpPost("scale-loads")]
    public async Task<IActionResult> GetScaleLoads([FromBody] ScaleLoadsRequest request)
    {
        var loads = await _scaleiService.GetScaleLoadsAsync(request);
        return Ok(loads);
    }

    /// <summary>POST /captures — Get captures/images for a specific weight</summary>
    [HttpPost("captures")]
    public async Task<IActionResult> GetLoadCaptures([FromBody] LoadCapturesRequest request)
    {
        var captures = await _scaleiService.GetLoadCapturesAsync(request.IdWeight);
        return Ok(captures);
    }

    /// <summary>POST /scale-images — Summary of images by dairy/date</summary>
    [HttpPost("scale-images")]
    public async Task<IActionResult> GetScaleImages([FromBody] ScaleImagesRequest request)
    {
        var images = await _scaleiService.GetScaleImagesAsync(request.StartDate, request.EndDate);
        return Ok(images);
    }

    /// <summary>POST /store-image — Store a single image/evidence</summary>
    [HttpPost("store-image")]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> StoreImage([FromBody] StoreImageRequest request)
    {
        await _scaleiService.StoreImageAsync(UserId!, request);
        return Ok(new { status = "ok" });
    }

    /// <summary>POST /store-images-bulk — Store up to 10 images. Returns 200 or 207.</summary>
    [HttpPost("store-images-bulk")]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> StoreImagesBulk([FromBody] StoreImagesBulkRequest request)
    {
        var result = await _scaleiService.StoreImagesBulkAsync(UserId!, request.Items);

        var statusCode = result.Failed > 0
            ? StatusCodes.Status207MultiStatus
            : StatusCodes.Status200OK;

        return StatusCode(statusCode, result);
    }

    /// <summary>POST /store-document — Store a document for a weight</summary>
    [HttpPost("store-document")]
    [Authorize(Policy = Policies.TenantWriter)]
    public async Task<IActionResult> StoreDocument([FromBody] StoreDocumentRequest request)
    {
        await _scaleiService.StoreDocumentAsync(UserId!, request);
        return Ok(new { status = "ok" });
    }

    /// <summary>POST /list-document-types — BOL, Invoice, etc. From General_Dairyi</summary>
    [HttpPost("list-document-types")]
    public async Task<IActionResult> ListDocumentTypes()
    {
        var types = await _scaleiService.GetDocumentTypesAsync();
        return Ok(types);
    }
}
