namespace NOW.FeatureFlagExtensions.FeatureManagement.Configuration
{
    public class AggregateFilterSettings
    {
        public string[]? Environments { get; set; }

        public string[]? RequiredClaims { get; set; }

        public string[]? RequiredHeaders { get; set; }
    }
}