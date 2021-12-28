namespace NOW.FeatureFlagExtensions.ApiVersioning.Configuration
{
    public class ApiVersioningOptions
    {
        public VersionOptions? DefaultVersion { get; set; }

        public ApiVersionDescriptionOptions? ApiDescription { get; set; }
    }
}