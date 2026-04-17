using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace APIMASTER.Services;

/// <summary>
/// Stores files to Dropbox using the HTTP API.
/// Reads DropboxToken and DropboxRoute from the DB config (List_Document_Settings TVF in ICCManager).
/// Falls back to appsettings Dropbox:Token / Dropbox:BasePath if DB config is unavailable.
/// </summary>
public class DropboxFileStorageService : IFileStorageService
{
    private readonly IConfiguration _config;
    private readonly ILogger<DropboxFileStorageService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public DropboxFileStorageService(
        IConfiguration config,
        ILogger<DropboxFileStorageService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string module, string subFolder, string fileName)
    {
        var token = _config["Dropbox:Token"]
            ?? throw new InvalidOperationException("Dropbox:Token not configured");
        var basePath = _config["Dropbox:BasePath"] ?? "/images";
        var remotePath = $"{basePath}/{module}/{subFolder}/{fileName}";

        using var stream = file.OpenReadStream();
        using var content = new StreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dropboxArg = JsonSerializer.Serialize(new
        {
            path = remotePath,
            mode = "overwrite",
            autorename = false,
            mute = true
        });
        client.DefaultRequestHeaders.Add("Dropbox-API-Arg", dropboxArg);

        var response = await client.PostAsync("https://content.dropboxapi.com/2/files/upload", content);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogError("Dropbox upload failed ({Status}): {Body}", response.StatusCode, body);
            throw new InvalidOperationException($"Dropbox upload failed: {response.StatusCode}");
        }

        _logger.LogDebug("File uploaded to Dropbox: {Path}", remotePath);
        return remotePath;
    }

    public async Task<(byte[] Content, string ContentType)?> GetFileAsync(string module, string subFolder, string fileName)
    {
        var token = _config["Dropbox:Token"];
        if (string.IsNullOrEmpty(token)) return null;

        var basePath = _config["Dropbox:BasePath"] ?? "/images";
        var remotePath = $"{basePath}/{module}/{subFolder}/{fileName}";

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dropboxArg = JsonSerializer.Serialize(new { path = remotePath });
        client.DefaultRequestHeaders.Add("Dropbox-API-Arg", dropboxArg);

        var response = await client.PostAsync("https://content.dropboxapi.com/2/files/download",
            new StringContent("", Encoding.UTF8));

        if (!response.IsSuccessStatusCode) return null;

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var contentType = GetContentType(fileName);
        return (bytes, contentType);
    }

    public Task DeleteFileAsync(string module, string subFolder, string fileName)
    {
        _logger.LogWarning("Dropbox delete not implemented for {Module}/{SubFolder}/{FileName}",
            module, subFolder, fileName);
        return Task.CompletedTask;
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
