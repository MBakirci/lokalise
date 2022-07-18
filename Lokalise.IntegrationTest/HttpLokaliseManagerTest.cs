using System;
using System.IO;
using System.Threading.Tasks;
using Lokalise.HTTP;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Lokalise.IntegrationTest;

public class HttpLokaliseManagerTest
{
    private IConfiguration _configuration;

    [OneTimeSetUp]
    public void Init()
    {
        _configuration = Util.BuildConfiguration(TestContext.CurrentContext.TestDirectory);
    }
    
    [SetUp]
    public void Setup()
    {
        var basePath = Environment.CurrentDirectory;
        const string fileName = "locale.zip";
        var filePath = Path.Combine(basePath, fileName);
        var dirPath = Path.Combine(basePath, "locale");
        
        if (File.Exists(filePath))
        {
            File.Delete(Path.Combine(filePath));
        }

        if (Directory.Exists(dirPath))
        {
            Directory.Delete(dirPath, true);
        }
    }
    
    [Test]
    public async Task TestDownload()
    {
        var manager = new HttpLokalise();
        var token = _configuration["Token"];
        var projectId = _configuration["ProjectId"];
        
        await manager.Download(token, projectId, DownloadFormat.Json);
        Assert.True(Directory.Exists(Path.Combine(Environment.CurrentDirectory, "locale")));
    }

    [Test]
    public async Task TestDownloadGivenDownloadPath()
    {
        var manager = new HttpLokalise(_configuration["DownloadPath"]);
        var token = _configuration["Token"];
        var projectId = _configuration["ProjectId"];
        
        await manager.Download(token, projectId, DownloadFormat.Json);
        Assert.True(Directory.Exists(Path.Combine(_configuration["DownloadPath"], "locale")));
    }

    [Test]
    public async Task TestDownloadGivenEmptyDownloadPath()
    {
        var manager = new HttpLokalise("");
        var token = _configuration["Token"];
        var projectId = _configuration["ProjectId"];
        
        await manager.Download(token, projectId, DownloadFormat.Json);
        Assert.True(Directory.Exists(Path.Combine(Environment.CurrentDirectory, "locale")));
    }
}