using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.DairyMobile;

[Route("api/v1/dairy/milk")]
[Authorize(Policy = Policies.TenantReader)]
public class MilkController : BaseController
{
    private readonly IMilkService _milkService;

    public MilkController(IMilkService milkService)
    {
        _milkService = milkService;
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers([FromQuery] MilkCustomersRequest request)
    {
        var (items, totalCount) = await _milkService.GetCustomersAsync(
            request.StartDate, request.EndDate, request.LocationId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No customers found for the specified criteria.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("dairies")]
    public async Task<IActionResult> GetDairies([FromQuery] MilkDairiesRequest request)
    {
        var (items, totalCount) = await _milkService.GetDairiesAsync(
            request.StartDate, request.EndDate, request.LocationId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No dairies found for the specified criteria.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("loads")]
    public async Task<IActionResult> GetLoads([FromQuery] MilkLoadsRequest request)
    {
        var (items, totalCount) = await _milkService.GetLoadsAsync(
            request.StartDate, request.EndDate, request.DairyId, request.CustomerLocationId,
            request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No loads found for the specified criteria.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("loads/search")]
    public async Task<IActionResult> SearchLoads([FromQuery] MilkLoadsSearchRequest request)
    {
        var (items, totalCount) = await _milkService.SearchLoadsAsync(
            request.Ticket, request.Manifest, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No loads found matching the search criteria.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("summary/customer")]
    public async Task<IActionResult> GetSummaryByCustomer([FromQuery] MilkSummaryRequest request)
    {
        var (items, totalCount) = await _milkService.GetSummaryByCustomerAsync(
            request.StartDate, request.EndDate, request.DairyId, request.DeliveryLocation,
            request.Group, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("summary/customer-dairy")]
    public async Task<IActionResult> GetSummaryByCustomerDairy([FromQuery] MilkSummaryRequest request)
    {
        var (items, totalCount) = await _milkService.GetSummaryByCustomerDairyAsync(
            request.StartDate, request.EndDate, request.Group, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("delivery-status")]
    public async Task<IActionResult> GetDeliveryMonthlyStatus([FromQuery] MilkDeliveryStatusRequest request)
    {
        var (items, totalCount) = await _milkService.GetDeliveryMonthlyStatusAsync(
            request.StartDate, request.EndDate, request.CustomerId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No delivery status data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("images/{milkLoadId:int}")]
    public async Task<IActionResult> GetImages(int milkLoadId)
    {
        var images = await _milkService.GetImagesAsync(milkLoadId);

        if (!images.Any())
            return NotFoundResponse("No images found for the specified milk load.");

        return OkResponse(images);
    }
}
