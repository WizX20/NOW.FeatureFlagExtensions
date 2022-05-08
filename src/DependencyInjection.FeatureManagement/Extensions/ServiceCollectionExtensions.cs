using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers.Extensions;

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

            // Register Microsoft Feature Management Manager.
            services.AddFeatureFlagManager<FeatureFlagFeatureManagementManager>();

            return services;
        }
    }
}