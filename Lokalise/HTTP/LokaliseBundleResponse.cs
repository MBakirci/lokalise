using Newtonsoft.Json;

namespace Lokalise.HTTP;

public class LokaliseBundleResponse
{
    [JsonProperty("project_id")]
    public string? ProjectId { get; set; }

    [JsonProperty("bundle_url")]
    public Uri? BundleUrl { get; set; }
}