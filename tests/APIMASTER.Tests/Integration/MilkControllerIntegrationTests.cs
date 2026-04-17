using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using APIMASTER.Models.Responses;

namespace APIMASTER.Tests.Integration;

public class MilkControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public MilkControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/customers
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCustomers_WithValidParams_ReturnsOkWithPaginatedResponse()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.True(body.TryGetProperty("data", out _));
        Assert.True(body.TryGetProperty("pagination", out var pagination));
        Assert.True(body.TryGetProperty("requestId", out _));
        Assert.True(pagination.GetProperty("totalCount").GetInt32() >= 0);
    }

    [Fact]
    public async Task GetCustomers_WithNoResults_Returns404()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=1900-01-01&endDate=1900-01-02&locationId=999999&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(404, body.Status);
        Assert.Contains("No customers found", body.Detail);
    }

    [Fact]
    public async Task GetCustomers_WithMissingRequiredParams_Returns400()
    {
        // locationId=0 fails validation (must be > 0)
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2024-01-01&endDate=2026-04-15&locationId=0&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_WithEndDateBeforeStartDate_Returns400()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2026-01-01&endDate=2024-01-01&locationId=1&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/dairies
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetDairies_WithValidParams_ReturnsOkWithPaginatedResponse()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/dairies?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.True(body.TryGetProperty("data", out _));
        Assert.True(body.TryGetProperty("pagination", out _));
    }

    [Fact]
    public async Task GetDairies_WithNoResults_Returns404()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/dairies?startDate=1900-01-01&endDate=1900-01-02&locationId=999999&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/loads
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetLoads_WithValidParams_ReturnsOkOrNotFound()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/loads?startDate=2024-01-01&endDate=2026-04-15&dairyId=1&customerLocationId=1&page=1&pageSize=10");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 200 or 404, got {(int)response.StatusCode}");
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/loads/search
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SearchLoads_WithTicket_ReturnsOkOrNotFound()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/loads/search?ticket=12345&page=1&pageSize=10");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 200 or 404, got {(int)response.StatusCode}");
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/delivery-status
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetDeliveryStatus_WithValidParams_ReturnsOkOrNotFound()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/delivery-status?startDate=2024-01-01&endDate=2026-04-15&customerId=1&page=1&pageSize=10");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 200 or 404, got {(int)response.StatusCode}");
    }

    // ──────────────────────────────────────────────
    // GET /api/v1/dairy/milk/images/{milkLoadId}
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetImages_WithNonExistentId_Returns404()
    {
        var response = await _client.GetAsync("/api/v1/dairy/milk/images/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ──────────────────────────────────────────────
    // Full pipeline validation
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Response_IncludesRequestId()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.True(body.TryGetProperty("requestId", out var requestId),
            "Response missing requestId");
        Assert.False(string.IsNullOrEmpty(requestId.GetString()),
            "requestId is empty");
    }

    [Fact]
    public async Task Response_ReturnsJsonContentType()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Response_IncludesSecurityHeaders()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/customers?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.True(response.Headers.Contains("X-Frame-Options"));
    }
}
