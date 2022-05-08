using Microsoft.Extensions.DependencyInjection;
using MicrosoftDependencyInjection = Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

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

            MicrosoftDependencyInjection.AddSingleton<IFeatureFlagManager, TImplementation>(services);

            return services;
        }
    }
}