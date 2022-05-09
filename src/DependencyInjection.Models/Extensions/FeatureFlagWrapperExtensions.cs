using Microsoft.Extensions.DependencyInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Models.Extensions
{
    public static class FeatureFlagWrapperExtensions
    {
        public static void StoreInServiceImplementationTypesCollection<TService>(
            this FeatureFlagWrapper<TService>[] implementations,
            IServiceProvider provider)
            where TService : class
        {
            var serviceType = typeof(TService);
            var serviceImplementationTypesCollection = provider.GetRequiredService<IServiceImplementationTypesCollection>();

            foreach (var implementation in implementations)
            {
                serviceImplementationTypesCollection.AddOrUpdate(serviceType, implementation.ImplementationType, implementation.Feature);
            }
        }

        public static void StoreInServiceImplementationTypesCollection<TService>(
            this FeatureFlagWrapper<TService> implementation,
            IServiceProvider provider)
            where TService : class
        {
            var serviceType = typeof(TService);
            var serviceImplementationTypesCollection = provider.GetRequiredService<IServiceImplementationTypesCollection>();

            serviceImplementationTypesCollection.AddOrUpdate(serviceType, implementation.ImplementationType, implementation.Feature);
        }
    }
}