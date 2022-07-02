using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NOW.FeatureFlagExtensions.Benchmarks.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var configuration = GetConfiguration();
            services.AddSingleton<IConfiguration>(configuration);

            return services;
        }

        private static IConfiguration GetConfiguration()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var applicationPath = Path.GetDirectoryName(assemblyPath);

            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(applicationPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            return configuration;
        }
    }
}