using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.Cross;

[Route("api/v1/cross/captures")]
[Authorize(Policy = Policies.TenantReader)]
public class CaptureController : BaseController
{
    private readonly ICrossCaptureService _captureService;

    public CaptureController(ICrossCaptureService captureService)
    {
        _captureService = captureService;
    }

    [HttpGet("milk-loads")]
    public async Task<IActionResult> GetMilkLoads([FromQuery] CrossUserLoadsRequest request)
    {
        var (items, totalCount) = await _captureService.GetMilkLoadsAsync(
            request.UserId, request.DaysBefore, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No milk loads found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("milk-loads/{milkLoadId:int}/images")]
    public async Task<IActionResult> GetMilkLoadImages(int milkLoadId)
    {
        var images = await _captureService.GetMilkLoadImagesAsync(milkLoadId);

        if (!images.Any())
            return NotFoundResponse("No images found for the specified milk load.");

        return OkResponse(images);
    }

    [HttpPost("milk-loads")]
    [Authorize(Policy = Policies.TenantWriter)]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(15 * 1024 * 1024)] // 15 MB
    public async Task<IActionResult> UpsertMilkLoad([FromForm] UpsertMilkLoadRequest request)
    {
        var milkLoadId = await _captureService.UpsertMilkLoadAsync(request);

        return CreatedAtAction(
            nameof(GetMilkLoadImages),
            new { milkLoadId },
            new { id = milkLoadId, requestId = RequestId });
    }

    [HttpPost("milk-loads/with-images")]
    [Authorize(Policy = Policies.TenantWriter)]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(200 * 1024 * 1024)] // 200 MB for multiple images
    public async Task<IActionResult> UpsertMilkLoadWithImages([FromForm] UpsertMilkLoadWithImagesRequest request)
    {
        var milkLoadId = await _captureService.UpsertMilkLoadWithImagesAsync(request);

        return CreatedAtAction(
            nameof(GetMilkLoadImages),
            new { milkLoadId },
            new { id = milkLoadId, requestId = RequestId });
    }

    [HttpPost("milk-loads/{milkLoadId:int}/images/others")]
    [Authorize(Policy = Policies.TenantWriter)]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(200 * 1024 * 1024)]
    public async Task<IActionResult> UpdateMilkLoadImagesOthers(
        int milkLoadId, [FromForm] UpdateMilkLoadImagesOthersRequest request)
    {
        request.IdMilkLoad = milkLoadId;
        await _captureService.UpdateMilkLoadImagesOthersAsync(request);

        return Ok(new { message = "Images updated successfully.", requestId = RequestId });
    }

    [HttpGet("silo-status")]
    public async Task<IActionResult> GetSiloStatusLoads([FromQuery] CrossUserLoadsRequest request)
    {
        var (items, totalCount) = await _captureService.GetSiloStatusLoadsAsync(
            request.UserId, request.DaysBefore, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No silo status loads found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("silo-status/{siloStatusLoadId:int}/images")]
    public async Task<IActionResult> GetSiloStatusImages(int siloStatusLoadId)
    {
        var images = await _captureService.GetSiloStatusImagesAsync(siloStatusLoadId);

        if (!images.Any())
            return NotFoundResponse("No images found for the specified silo status.");

        return OkResponse(images);
    }

    [HttpPost("silo-status")]
    [Authorize(Policy = Policies.TenantWriter)]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(200 * 1024 * 1024)]
    public async Task<IActionResult> UpsertSiloStatusWithImages([FromForm] UpsertSiloStatusRequest request)
    {
        var siloStatusId = await _captureService.UpsertSiloStatusWithImagesAsync(request);

        return CreatedAtAction(
            nameof(GetSiloStatusImages),
            new { siloStatusLoadId = siloStatusId },
            new { id = siloStatusId, requestId = RequestId });
    }
}
