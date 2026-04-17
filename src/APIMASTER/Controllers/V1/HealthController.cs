using APIMASTER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIMASTER.Controllers.V1;

[Route("health")]
[ApiController]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    private readonly IDatabaseResolver _dbResolver;
    private readonly ILogger<HealthController> _logger;

    private static readonly string[] DatabaseNames =
        ["Cross", "GeneralDairyi", "General", "ApprovalProd", "iScaleOutlier", "ICCManager"];

    public HealthController(IDatabaseResolver dbResolver, ILogger<HealthController> logger)
    {
        _dbResolver = dbResolver;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check - returns 200 if the API is running.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// Detailed health check - verifies connectivity to all databases.
    /// </summary>
    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailed()
    {
        var results = new Dictionary<string, object>();
        var allHealthy = true;

        foreach (var dbName in DatabaseNames)
        {
            try
            {
                using var conn = _dbResolver.GetConnectionByName(dbName);
                await conn.OpenAsync();
                results[dbName] = new { status = "healthy" };
            }
            catch (Exception ex)
            {
                allHealthy = false;
                results[dbName] = new { status = "unhealthy", error = ex.Message };
                _logger.LogError(ex, "Health check failed for database {Database}", dbName);
            }
        }

        var response = new
        {
            status = allHealthy ? "healthy" : "degraded",
            timestamp = DateTime.UtcNow,
            databases = results
        };

        return allHealthy ? Ok(response) : StatusCode(503, response);
    }
}
