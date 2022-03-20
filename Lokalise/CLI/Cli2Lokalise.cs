using System.Runtime.InteropServices;
using System.Text;

namespace Lokalise.CLI;

public class Cli2Lokalise : ILokalise
{
    private string? processPath;
    public Cli2Lokalise(string lokalisePath = null)
    {
        processPath = lokalisePath ?? "lokalise2";
    }
    
    public async Task Download(string token, string projectId, DownloadFormat format)
    {
        var arg = new StringBuilder();
        arg.Append($"--token {token} ");
        arg.Append($"--project-id {projectId} ");
        arg.Append("file download ");
        arg.Append($"--format {format.ToString().ToLower()} ");
        arg.Append("--original-filenames=false ");
        arg.Append("--export-empty-as=base ");
        arg.Append("--unzip-to . ");
        
        await ProcessAsyncHelper.RunProcessAsync(command: processPath, arg.ToString(), -1);
    }
}