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

            var enabledFeatureFound = false;

            foreach (var (implementationType, feature) in serviceImplementationTypes.Value)
            {
                var featureIsEnabled = _featureFlagManager.IsEnabled(feature);
                if (!featureIsEnabled)
                {
                    continue;
                }

                enabledFeatureFound = true;

                var implementationObject = _serviceProvider.GetRequiredService(implementationType);
                var methodInfo = invocation.Method;
                var parameters = methodInfo.GetParameters();
                invocation.ReturnValue = methodInfo.Invoke(implementationObject, parameters);
            }

            if (!enabledFeatureFound)
            {
                invocation.Proceed();
            }
        }
    }
}