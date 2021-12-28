namespace FeatureTestApplication.Configuration
{
    public class FeatureTestApplicationOptions
    {
        public CorsPolicyOptions? CorsPolicy { get; set; }

        public LocalizationOptions? Localization { get; set; }

        public ResponseCachingOptions? ResponseCaching { get; set; }

        public SwaggerOptions? Swagger { get; set; }
    }
}