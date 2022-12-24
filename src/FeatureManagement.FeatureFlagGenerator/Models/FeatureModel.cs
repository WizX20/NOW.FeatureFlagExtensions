using Newtonsoft.Json;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Models
{
    [JsonObject]
    public class FeatureModel
    {
        public EnabledForModel? EnabledForModel { get; set; }
    }
}