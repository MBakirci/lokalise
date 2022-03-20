using System.Runtime.InteropServices;
using System.Text;

namespace Lokalise.CLI;

public class Cli2Lokalise : ILokalise
{
    private string? processPath;
    public Cli2Lokalise()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            processPath = Path.Combine(Environment.CurrentDirectory, "lokalise2_linux");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            processPath = Path.Combine(Environment.CurrentDirectory, "lokalise2_darwin");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            processPath = Path.Combine(Environment.CurrentDirectory, "lokalise2.exe");
        }
    }
    
    public async Task Download(string token, string projectId, DownloadFormat format)
    {
        var arg = new StringBuilder();
        arg.Append($"--token {token} ");
        arg.Append($"--project-id {projectId} ");
        arg.Append("file download ");
        arg.Append($"--format {format.ToString().ToLower()} ");
        arg.Append($"--bundle-structure 'locale/%LANG_ISO%.{format.ToString().ToLower()}'");
        arg.Append("--original-filenames=false ");
        arg.Append("--export-empty-as=base ");
        arg.Append("--unzip-to . ");
        
        await ProcessAsyncHelper.RunProcessAsync(command: processPath, arg.ToString(), -1);
    }
}