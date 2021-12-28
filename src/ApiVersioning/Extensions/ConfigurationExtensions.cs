using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NOW.FeatureFlagExtensions.ApiVersioning.Configuration;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ApiVersion GetDefaultApiVersion(
            this IConfigurationRoot configuration,
            string configurationSection,
            ApiVersion defaultApiVersionFallback,
            ILogger? logger = null)
        {
            try
            {
                var versionConfiguration = configuration.GetSection(
                    $"{configurationSection}:{nameof(ApiVersioningOptions.DefaultVersion)}"
                );

                var defaultApiVersion = new ApiVersion(
                    versionConfiguration.GetValue<int>(nameof(VersionOptions.Major)),
                    versionConfiguration.GetValue<int>(nameof(VersionOptions.Minor))
                );

                return defaultApiVersion;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Unable to load API version from configuration! Falling back on {Version}", defaultApiVersionFallback);
            }

            return defaultApiVersionFallback;
        }
    }
}