using Newtonsoft.Json;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Models
{
    [JsonObject("EnabledFor")]
    public class EnabledForModel
    {
        public string? Name { get; set; }

        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}