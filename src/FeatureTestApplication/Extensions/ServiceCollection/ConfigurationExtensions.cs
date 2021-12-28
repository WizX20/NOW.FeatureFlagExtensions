using FeatureTestApplication.Configuration;

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

            var appSettings = new AppSettingsConfiguration();
            configuration.Bind(appSettings);

            services.AddSingleton(appSettings);

            return appSettings;
        }
    }
}