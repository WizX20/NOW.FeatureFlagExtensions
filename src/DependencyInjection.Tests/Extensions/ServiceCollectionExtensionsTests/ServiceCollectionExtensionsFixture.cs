using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers.Extensions;
using TestFeatureFlagManager;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Tests.Extensions.ServiceCollectionExtensionsTests
{
    public static class ServiceCollectionExtensionsFixture
    {
        public static TestManager AddTestFeatureFlagManager(IServiceCollection services)
        {
            services.AddFeatureFlagManager<TestManager>();

            var serviceProvider = services.BuildServiceProvider();
            var manager = serviceProvider.GetService<IFeatureFlagManager>();
            var testManager = manager as TestManager;
            if (testManager == null)
            {
                throw new Exception($"Unable to add or get {nameof(TestManager)}");
            }

            return testManager;
        }
    }
}