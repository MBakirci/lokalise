namespace Lokalise;

public interface ILokalise
{
    Task Download(string token, string projectId, DownloadFormat format);
}