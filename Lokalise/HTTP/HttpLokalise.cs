using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lokalise.HTTP;

public class HttpLokalise : ILokalise
{
    private readonly HttpClient _httpClient = new();
    private const string FileName = "locale.zip";
    private string DownloadPath;
    private string ZipFilePath => Path.Combine(DownloadPath, FileName);
    private string DestinationPath => Path.Combine(DownloadPath, "locale");

    public HttpLokalise()
    {
        DownloadPath = Environment.CurrentDirectory;
        Cleanup();
    }

    public HttpLokalise(string currentDirectory)
    {
        DownloadPath = !string.IsNullOrEmpty(currentDirectory) ? currentDirectory : Environment.CurrentDirectory;
        Cleanup();
    }

    private void Cleanup()
    {
        if (File.Exists(ZipFilePath))
        {
            File.Delete(ZipFilePath);
        }

        if (Directory.Exists(DestinationPath))
        {
            Directory.Delete(DestinationPath, true);
        }
    }

    public async Task Download(string token, string projectId, DownloadFormat format)
    {
        var bundleInfo = await GetBundle(token, projectId, DownloadFormat.Json);
        if (bundleInfo == null) throw new MissingBundleException();
        await DownloadBundleZip(bundleInfo.BundleUrl);
        ZipFile.ExtractToDirectory(ZipFilePath, DestinationPath);
    }

    private async Task<LokaliseBundleResponse?> GetBundle(string token, string projectId, DownloadFormat format)
    {
        using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://api.lokalise.com/api2/projects/{projectId}/files/download");
        request.Headers.TryAddWithoutValidation("x-api-token", token); 

        var requestBody = new { format = "json", export_empty_as = "base" };
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json"); 

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LokaliseBundleResponse>(responseString);
    }
    
    private async Task DownloadBundleZip(Uri? bundleInfoBundleUrl)
    {
        if (bundleInfoBundleUrl == null)
        {
            throw new ArgumentNullException(nameof(bundleInfoBundleUrl));
        }
        var stream = await _httpClient.GetStreamAsync(bundleInfoBundleUrl);
        await using var fs = new FileStream(ZipFilePath, FileMode.CreateNew);
        await stream.CopyToAsync(fs);
    }
}