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
    }
}