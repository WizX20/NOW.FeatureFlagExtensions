namespace FeatureTestApplication.Extensions.WebHostEnvironment
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Builds and registers the `appsettings.json` configuration.
        /// </summary>
        /// <param name="webHostEnvironment">The web host environment.</param>
        public static IConfigurationRoot GetConfigurationRoot(this IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment is null)
            {
                throw new ArgumentNullException(nameof(webHostEnvironment));
            }

            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(webHostEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return configuration;
        }
    }
}