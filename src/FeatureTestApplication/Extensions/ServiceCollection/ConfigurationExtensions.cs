using FeatureTestApplication.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FeatureTestApplication.Extensions.ServiceCollection
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Builds and registers the `appsettings.json` configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        public static AppSettingsConfiguration RegisterAppSettingsConfiguration(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddOptions<AppSettingsConfiguration>()
                .Bind(configuration)
                .ValidateOnStart();

            var appSettings = new AppSettingsConfiguration();
            configuration.Bind(appSettings);

            return appSettings;
        }
    }
}