using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.DairyMobile;

[Route("api/v1/dairy/transfers")]
[Authorize(Policy = Policies.TenantReader)]
public class TransferController : BaseController
{
    private readonly ITransferService _transferService;

    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpGet("commodities")]
    public async Task<IActionResult> GetCommodities([FromQuery] TransferRequest request)
    {
        var (items, totalCount) = await _transferService.GetCommoditiesAsync(
            request.StartDate, request.EndDate, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No transfer commodities found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("dairies")]
    public async Task<IActionResult> GetDairies([FromQuery] TransferRequest request)
    {
        var (items, totalCount) = await _transferService.GetDairiesAsync(
            request.StartDate, request.EndDate, request.CommodityId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No transfer dairies found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("loads")]
    public async Task<IActionResult> GetLoads([FromQuery] TransferRequest request)
    {
        var (items, totalCount) = await _transferService.GetLoadsAsync(
            request.StartDate, request.EndDate, request.CommodityId, request.DairyId,
            request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No transfer loads found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("summary/commodity")]
    public async Task<IActionResult> GetSummaryCommodity([FromQuery] TransferSummaryRequest request)
    {
        var (items, totalCount) = await _transferService.GetSummaryCommodityAsync(
            request.StartDate, request.EndDate, request.Group, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No transfer summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("summary/route")]
    public async Task<IActionResult> GetSummaryRoute([FromQuery] TransferSummaryRequest request)
    {
        var (items, totalCount) = await _transferService.GetSummaryRouteAsync(
            request.StartDate, request.EndDate, request.Group, request.CommodityId,
            request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No transfer route summary found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }
}
