namespace NOW.FeatureFlagExtensions.ApiVersioning.Configuration
{
    public class ApiVersionDescriptionOptions
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public ApiContactOptions? Contact { get; set; }

        public ApiLicenseOptions? License { get; set; }
    }
}