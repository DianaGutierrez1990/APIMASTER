using APIMASTER.Authorization;
using APIMASTER.Models.Requests;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.DairyMobile;

[Route("api/v1/dairy/harvest")]
[Authorize(Policy = Policies.TenantReader)]
public class HarvestController : BaseController
{
    private readonly IHarvestService _harvestService;

    public HarvestController(IHarvestService harvestService)
    {
        _harvestService = harvestService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetHarvests([FromQuery] HarvestRequest request)
    {
        var (items, totalCount) = await _harvestService.GetHarvestsAsync(
            request.StartDate, request.EndDate, request.FieldId, request.Crop,
            request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No harvests found for the specified criteria.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("field-summary")]
    public async Task<IActionResult> GetFieldSummary([FromQuery] FieldSummaryRequest request)
    {
        var (items, totalCount) = await _harvestService.GetFieldSummaryAsync(
            request.StartDate, request.EndDate, request.Crop, request.Field,
            request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No field summary data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> GetSummaryFilters()
    {
        var filters = await _harvestService.GetSummaryFiltersAsync();

        if (!filters.Any())
            return NotFoundResponse("No filters available.");

        return OkResponse(filters);
    }

    [HttpGet("drymatter")]
    public async Task<IActionResult> GetFieldDryMatter([FromQuery] FieldSummaryRequest request)
    {
        var (items, totalCount) = await _harvestService.GetFieldDryMatterAsync(
            request.StartDate, request.EndDate, request.Field, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No dry matter data found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("loads")]
    public async Task<IActionResult> GetFieldLoads([FromQuery] HarvestRequest request)
    {
        var (items, totalCount) = await _harvestService.GetFieldLoadsAsync(
            request.StartDate, request.EndDate, request.FieldId, request.Skip, request.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No field loads found.");

        return OkPaginated(items, totalCount, request.Page, request.PageSize);
    }

    [HttpGet("trucks")]
    public async Task<IActionResult> GetTrucks([FromQuery] PaginationParams pagination)
    {
        var (items, totalCount) = await _harvestService.GetTrucksAsync(
            pagination.Skip, pagination.PageSize);

        if (totalCount == 0)
            return NotFoundResponse("No trucks found.");

        return OkPaginated(items, totalCount, pagination.Page, pagination.PageSize);
    }
}
