using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors
{
    public class ManagedInterceptor : IManagedInterceptor
    {
        private readonly IFeatureFlagManager _featureFlagManager;
        private readonly IServiceImplementationTypesCollection _serviceImplementationTypes;
        private readonly IServiceProvider _serviceProvider;

        public ManagedInterceptor(
            IFeatureFlagManager featureFlagManager,
            IServiceImplementationTypesCollection serviceImplementationTypes,
            IServiceProvider serviceProvider)
        {
            _featureFlagManager = featureFlagManager ?? throw new ArgumentNullException(nameof(featureFlagManager));
            _serviceImplementationTypes = serviceImplementationTypes ?? throw new ArgumentNullException(nameof(serviceImplementationTypes));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Intercept(IInvocation invocation)
        {
            var targetType = invocation.TargetType;
            var serviceImplementationTypes = _serviceImplementationTypes.GetServiceTypeAssignableFrom(targetType);

            var targetTypeIsFeatureImplementation = false;

            foreach (var (implementationType, feature) in serviceImplementationTypes.Value)
            {
                if (!targetTypeIsFeatureImplementation && implementationType == targetType)
                {
                    targetTypeIsFeatureImplementation = true;
                }

                var featureIsEnabled = _featureFlagManager.IsEnabled(feature);
                if (featureIsEnabled)
                {
                    if (targetTypeIsFeatureImplementation)
                    {
                        invocation.Proceed();
                        return;
                    }

                    InvokeType(invocation, implementationType);
                    return;
                }
            }

            if (targetTypeIsFeatureImplementation)
            {
                var services = _serviceProvider.GetServices(serviceImplementationTypes.Key);
                var implementation = services.FirstOrDefault(s =>
                    s != null &&
                    !serviceImplementationTypes.Value.Any(si => si.ImplementationType == s.GetType())
                );

                if (implementation != null)
                {
                    InvokeType(invocation, implementation.GetType());
                    return;
                }
            }

            invocation.Proceed();
        }

        private void InvokeType(IInvocation invocation, Type? type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var implementationObject = _serviceProvider.GetRequiredService(type);
            var methodInfo = invocation.Method;
            var parameters = methodInfo.GetParameters();
            invocation.ReturnValue = methodInfo.Invoke(implementationObject, parameters);
        }
    }
}