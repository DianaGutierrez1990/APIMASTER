using System.Security.Claims;
using APIMASTER.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected string RequestId => HttpContext.TraceIdentifier;

    /// <summary>
    /// Extracts the authenticated user's ID from the JWT "sub" claim.
    /// Returns null if not authenticated or claim is missing.
    /// </summary>
    protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue("sub");

    protected IActionResult OkPaginated<T>(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        return Ok(PaginatedResponse<T>.Create(items, totalCount, page, pageSize, RequestId));
    }

    protected IActionResult OkResponse<T>(T data)
    {
        return Ok(ApiResponse<T>.Success(data, RequestId));
    }

    protected IActionResult NotFoundResponse(string detail)
    {
        return NotFound(ErrorResponse.NotFound(detail, RequestId));
    }

    protected IActionResult ValidationErrorResponse(Dictionary<string, string[]> errors)
    {
        return BadRequest(ErrorResponse.Validation(errors, RequestId));
    }
}
