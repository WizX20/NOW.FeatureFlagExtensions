using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureFlagFeatureManagementManager(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddFeatureFlagManager<FeatureFlagFeatureManagementManager>();

            return services;
        }
    }
}