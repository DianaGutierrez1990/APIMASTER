namespace APIMASTER.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Saves a file to local storage and FTP, returns the relative path.
    /// </summary>
    Task<string> SaveFileAsync(IFormFile file, string module, string subFolder, string fileName);

    /// <summary>
    /// Downloads a file from FTP and returns the byte array with content type.
    /// </summary>
    Task<(byte[] Content, string ContentType)?> GetFileAsync(string module, string subFolder, string fileName);

    /// <summary>
    /// Deletes a file from both local and FTP storage.
    /// </summary>
    Task DeleteFileAsync(string module, string subFolder, string fileName);
}
