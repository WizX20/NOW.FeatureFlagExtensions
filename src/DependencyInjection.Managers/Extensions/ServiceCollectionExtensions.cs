using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Managers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureFlagManager<TImplementation>(
            this IServiceCollection services)
            where TImplementation : class, IFeatureFlagManager
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IFeatureFlagManager, TImplementation>();

            return services;
        }
    }
}