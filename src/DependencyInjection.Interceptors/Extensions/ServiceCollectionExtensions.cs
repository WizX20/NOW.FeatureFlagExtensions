using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using NOW.FeatureFlagExtensions.DependencyInjection.Models.Extensions;
using MicrosoftDependencyInjection = Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterceptedScoped<TService, TImplementation>(
            this IServiceCollection services,
            params FeatureFlagWrapper<TService>[] implementations)
            where TService : class
            where TImplementation : class, TService
        {
            Guard(services, implementations);

            RegisterRequiredServices(services);

            // Register default implementation.
            MicrosoftDependencyInjection.AddScoped<TImplementation>(services);
            MicrosoftDependencyInjection.AddScoped<TService, TImplementation>(services);
            MicrosoftDependencyInjection.AddScoped(services, provider =>
            {
                implementations.StoreInServiceImplementationTypesCollection(provider);

                return provider.GetInterfaceProxyWithTarget<TService, TImplementation>();
            });

            // Register feature implementations.
            var serviceType = typeof(TService);
            foreach (var implementation in implementations)
            {
                MicrosoftDependencyInjection.AddScoped(services, serviceType, implementation.ImplementationType);
            }

            return services;
        }

        public static IServiceCollection AddInterceptedSingleton<TService, TImplementation>(
            this IServiceCollection services,
            params FeatureFlagWrapper<TService>[] implementations)
            where TService : class
            where TImplementation : class, TService
        {
            Guard(services, implementations);

            RegisterRequiredServices(services);

            // Register default implementation.
            MicrosoftDependencyInjection.AddSingleton<TImplementation>(services);
            MicrosoftDependencyInjection.AddSingleton<TService, TImplementation>(services);
            MicrosoftDependencyInjection.AddSingleton(services, provider =>
            {
                implementations.StoreInServiceImplementationTypesCollection(provider);

                return provider.GetInterfaceProxyWithTarget<TService, TImplementation>();
            });

            // Register feature implementations.
            var serviceType = typeof(TService);
            foreach (var implementation in implementations)
            {
                MicrosoftDependencyInjection.AddSingleton(services, serviceType, implementation.ImplementationType);
            }

            return services;
        }

        public static IServiceCollection AddInterceptedTransient<TService, TImplementation>(
            this IServiceCollection services,
            params FeatureFlagWrapper<TService>[] implementations)
            where TService : class
            where TImplementation : class, TService
        {
            Guard(services, implementations);

            RegisterRequiredServices(services);

            // Register default implementation.
            MicrosoftDependencyInjection.AddTransient<TImplementation>(services);
            MicrosoftDependencyInjection.AddTransient<TService, TImplementation>(services);
            MicrosoftDependencyInjection.AddTransient(services, provider =>
            {
                implementations.StoreInServiceImplementationTypesCollection(provider);

                return provider.GetInterfaceProxyWithTarget<TService, TImplementation>();
            });

            // Register feature implementations.
            var serviceType = typeof(TService);
            foreach (var implementation in implementations)
            {
                MicrosoftDependencyInjection.AddTransient(services, serviceType, implementation.ImplementationType);
            }

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

        private static void RegisterRequiredServices(IServiceCollection services)
        {
            // Register castle proxy generator, if not already registered.
            services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

            // Register the collection that keeps track of all created types.
            services.TryAddSingleton<IServiceImplementationTypesCollection, ServiceImplementationTypesCollection>();

            // Register the interceptor, if not already registered.
            services.TryAddTransient<IManagedInterceptor, ManagedInterceptor>();
        }
    }
}