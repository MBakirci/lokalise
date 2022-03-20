using Microsoft.Extensions.Configuration;

namespace Lokalise.IntegrationTest;

public static class Util
{
    public static IConfigurationRoot BuildConfiguration(string testDirectory)
    {
        return new ConfigurationBuilder()
            .SetBasePath(testDirectory)
            .AddJsonFile("appsettings.test.json", optional: true)
            .Build();
    }
}