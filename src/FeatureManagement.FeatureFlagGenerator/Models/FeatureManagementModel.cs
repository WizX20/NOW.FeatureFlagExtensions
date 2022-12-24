using Newtonsoft.Json;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Models
{
    [JsonObject("FeatureManagement")]
    public class FeatureManagementModel
    {
        public Dictionary<string, object> Features { get; set; } = new Dictionary<string, object>();
    }
}