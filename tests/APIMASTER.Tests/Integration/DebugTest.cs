using System.Net.Http.Json;
using System.Text.Json;

namespace APIMASTER.Tests.Integration;

public class DebugTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DebugTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Debug_GetDairies_ShowErrorBody()
    {
        var response = await _client.GetAsync(
            "/api/v1/dairy/milk/dairies?startDate=2024-01-01&endDate=2026-04-15&locationId=1&page=1&pageSize=10");

        var body = await response.Content.ReadAsStringAsync();
        // This will show the error in the test output
        Assert.True(false, $"Status: {response.StatusCode}, Body: {body}");
    }

    [Fact]
    public async Task Debug_GetImages_ShowErrorBody()
    {
        var response = await _client.GetAsync("/api/v1/dairy/milk/images/999999");

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(false, $"Status: {response.StatusCode}, Body: {body}");
    }
}
