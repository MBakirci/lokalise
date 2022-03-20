using System;
using System.IO;
using System.Threading.Tasks;
using Lokalise.CLI;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Lokalise.IntegrationTest;

public class Cli2LokaliseManagerTest
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
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.test.json", false, true)
            .Build();
    }

    [Test]
    public async Task TestDownload()
    {
        var manager = new Cli2Lokalise();
        var token = _configuration["Token"];
        var projectId = _configuration["ProjectId"];
        
        await manager.Download(token, projectId, DownloadFormat.Json);
        Assert.True(Directory.Exists(Path.Combine(Environment.CurrentDirectory, "locale")));
    }
}