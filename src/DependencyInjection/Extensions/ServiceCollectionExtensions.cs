using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using MicrosoftDependencyInjection = Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScoped<TService, TImplementation>(
            this IServiceCollection services,
            params FeatureFlagWrapper<TService>[] implementations)
            where TService : class
            where TImplementation : class, TService
        {
            Guard(services, implementations);

            // Register default implementation.
            MicrosoftDependencyInjection.AddScoped<TImplementation>(services);

            // Register feature implementations.
            foreach (var implementation in implementations)
            {
                MicrosoftDependencyInjection.AddScoped(services, implementation.ImplementationType);
            }

            // Try and find the registered implementation by feature, when the interface is requested.
            MicrosoftDependencyInjection.AddScoped(services, provider => GetImplementation<TService, TImplementation>(implementations, provider));

            return services;
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(
            this IServiceCollection services,
            params FeatureFlagWrapper<TService>[] implementations)
            where TService : class
            where TImplementation : class, TService
        {
            Guard(services, implementations);

            // Register default implementation.
            MicrosoftDependencyInjection.AddTransient<TImplementation>(services);

            // Register feature implementations.
            foreach (var implementation in implementations)
            {
                MicrosoftDependencyInjection.AddTransient(services, implementation.ImplementationType);
            }

            // Try and find the registered implementation by feature, when the interface is requested.
            MicrosoftDependencyInjection.AddTransient(services, provider => GetImplementation<TService, TImplementation>(implementations, provider));

            return services;
        }

        private static void Guard<TService>(
            IServiceCollection services,
            FeatureFlagWrapper<TService>[] implementations)
            where TService : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (implementations == null)
            {
                throw new ArgumentNullException(nameof(implementations));
            }

            if (implementations.Length < 1)
            {
                throw new ArgumentException(
                    $"At least one featured implementation is required in the '{nameof(implementations)}' collection.",
                    nameof(implementations)
                );
            }
        }

        private static TService GetImplementation<TService, TImplementation>(
            FeatureFlagWrapper<TService>[] implementations,
            IServiceProvider provider)
            where TService : class
            where TImplementation : class, TService
        {
            var featureManager = provider.GetRequiredService<IFeatureFlagManager>();

            foreach (var implementation in implementations)
            {
                var featureIsEnabled = featureManager.IsEnabled(implementation.Feature);
                if (featureIsEnabled)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return provider.GetService(implementation.ImplementationType) as TService;
#pragma warning restore CS8603 // Possible null reference return.
                }
            }

            // Default implementation.
#pragma warning disable CS8603 // Possible null reference return.
            return provider.GetService<TImplementation>();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}