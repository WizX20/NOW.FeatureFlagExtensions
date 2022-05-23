using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.DefaultInjection;
using Flags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.DefaultInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Tests.Extensions.ServiceCollectionExtensionsTests
{
    public static class ServiceCollectionExtensionsFixture
    {
        public static FeatureFlagWrapper<Features.ScopedFeature.ITestServiceScoped>[] SetScopedFeatures(
            IServiceCollection services,
            bool addTestServiceTwo = false,
            bool enableTestServiceTwo = false,
            bool addTestServiceThree = false,
            bool enableTestServiceThree = false)
        {
            var testManagerState = new TestManagerState();
            var implementations = new List<FeatureFlagWrapper<Features.ScopedFeature.ITestServiceScoped>>();

            if (addTestServiceTwo)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.ScopedFeature.ITestServiceScoped>(
                        typeof(Features.ScopedFeature.TestServiceTwoScoped),
                        Flags.TestDefaultServiceScopedTwo
                    )
                );

                testManagerState.Add(Flags.TestDefaultServiceScopedTwo, enableTestServiceTwo);
            }

            if (addTestServiceThree)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.ScopedFeature.ITestServiceScoped>(
                        typeof(Features.ScopedFeature.TestServiceThreeScoped),
                        Flags.TestDefaultServiceScopedThree
                    )
                );

                testManagerState.Add(Flags.TestDefaultServiceScopedThree, enableTestServiceThree);
            }

            services.AddSingleton<ITestManagerState>(testManagerState);

            AddTestFeatureFlagManager(services);

            return implementations.ToArray();
        }

        public static TestManager AddTestFeatureFlagManager(
            IServiceCollection services)
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