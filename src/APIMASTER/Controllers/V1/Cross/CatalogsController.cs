using APIMASTER.Authorization;
using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1.Cross;

[Route("api/v1/cross/catalogs")]
[Authorize(Policy = Policies.TenantReader)]
public class CatalogsController : BaseController
{
    private readonly ICrossCatalogService _catalogService;

    public CatalogsController(ICrossCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("customer-dairy-barn-silos")]
    public async Task<IActionResult> GetCustomerDairyBarnSilos()
    {
        var items = await _catalogService.GetCustomerDairyBarnSilosAsync();

        if (!items.Any())
            return NotFoundResponse("No customer dairy barn silos found.");

        return OkResponse(items);
    }

    [HttpGet("trucks")]
    public async Task<IActionResult> GetTrucks()
    {
        var items = await _catalogService.GetTrucksAsync();

        if (!items.Any())
            return NotFoundResponse("No trucks found.");

        return OkResponse(items);
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> GetDrivers()
    {
        var items = await _catalogService.GetDriversAsync();

        if (!items.Any())
            return NotFoundResponse("No drivers found.");

        return OkResponse(items);
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var items = await _catalogService.GetCatalogsByTypeAsync(type);

        if (!items.Any())
            return NotFoundResponse($"No catalogs found for type '{type}'.");

        return OkResponse(items);
    }
}
