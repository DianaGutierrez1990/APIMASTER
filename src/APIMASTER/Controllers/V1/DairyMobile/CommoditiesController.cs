using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.DairyMobile;

[Route("api/v1/dairy/commodities")]
[Authorize(Policy = Policies.TenantReader)]
public class CommoditiesController : BaseController
{
    private readonly ICommoditiesService _commoditiesService;

    public CommoditiesController(ICommoditiesService commoditiesService)
    {
        _commoditiesService = commoditiesService;
    }

    [HttpGet("pickup")]
    public async Task<IActionResult> GetPickup([FromQuery] CommoditiesRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetPickupAsync(
            request.StartDate, request.EndDate, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No commodity pickup data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("delivery")]
    public async Task<IActionResult> GetDelivery([FromQuery] CommoditiesRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetDeliveryAsync(
            request.StartDate, request.EndDate, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No commodity delivery data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("vendors")]
    public async Task<IActionResult> GetVendors([FromQuery] CommoditiesRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetVendorsAsync(
            request.StartDate, request.EndDate, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No vendors found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("loads")]
    public async Task<IActionResult> GetLoads([FromQuery] CommodityLoadsRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetLoadsAsync(
            request.StartDate, request.EndDate, request.CommodityId, request.VendorId,
            request.DeliveryLocation, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No commodity loads found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("tickets/search")]
    public async Task<IActionResult> FindTicket([FromQuery] TicketSearchRequest request)
    {
        var result = await _commoditiesService.FindTicketAsync(request.ScaleTicket);
        return OkResponse(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] CommoditySummaryRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetSummaryAsync(
            request.StartDate, request.EndDate, request.Group, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("summary/dairy")]
    public async Task<IActionResult> GetSummaryByDairy([FromQuery] CommoditySummaryRequest request)
    {
        var (items, totalCount) = await _commoditiesService.GetSummaryByDairyAsync(
            request.StartDate, request.EndDate, request.Group, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }
}
