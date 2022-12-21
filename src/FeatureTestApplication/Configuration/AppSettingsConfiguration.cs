using NOW.ApiVersioning.Configuration;

namespace FeatureTestApplication.Configuration
{
    public class AppSettingsConfiguration
    {
        public FeatureTestApplicationOptions? FeatureTestApplication { get; set; }

        public ApiVersioningOptions? ApiVersioning { get; set; }
    }
}