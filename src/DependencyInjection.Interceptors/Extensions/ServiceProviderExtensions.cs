using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TService GetInterfaceProxyWithTarget<TService, TImplementation>(this IServiceProvider provider)
            where TService : class
            where TImplementation : class, TService
        {
            var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
            var implementationTarget = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<IManagedInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationTarget, interceptor);
        }

        public static TService GetInterfaceProxyWithTarget<TService>(this IServiceProvider provider, Type implementationType)
            where TService : class
        {
            var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
            var implementationTarget = provider.GetRequiredService(implementationType) as TService;
            if (implementationTarget == null)
            {
                throw new InvalidOperationException($"The given {nameof(implementationType)} is not of the type {nameof(TService)}");
            }

            var interceptor = provider.GetRequiredService<IManagedInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget(implementationTarget, interceptor);
        }
    }
}