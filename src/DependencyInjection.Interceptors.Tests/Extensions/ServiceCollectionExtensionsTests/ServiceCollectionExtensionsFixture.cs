using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.InterceptorInjection;
using Flags = TestFeatures.FeatureFlagsGenerated.ReleaseFlags;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Tests.Extensions.ServiceCollectionExtensionsTests
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
                        Flags.TestInterceptedServiceScopedTwo
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceScopedTwo, enableTestServiceTwo);
            }

            if (addTestServiceThree)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.ScopedFeature.ITestServiceScoped>(
                        typeof(Features.ScopedFeature.TestServiceThreeScoped),
                        Flags.TestInterceptedServiceScopedThree
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceScopedThree, enableTestServiceThree);
            }

            services.AddSingleton<ITestManagerState>(testManagerState);

            AddTestFeatureFlagManager(services);

            return implementations.ToArray();
        }

        public static FeatureFlagWrapper<Features.SingletonFeature.ITestServiceSingleton>[] SetSingletonFeatures(
            IServiceCollection services,
            bool addTestServiceTwo = false,
            bool enableTestServiceTwo = false,
            bool addTestServiceThree = false,
            bool enableTestServiceThree = false)
        {
            var testManagerState = new TestManagerState();
            var implementations = new List<FeatureFlagWrapper<Features.SingletonFeature.ITestServiceSingleton>>();

            if (addTestServiceTwo)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.SingletonFeature.ITestServiceSingleton>(
                        typeof(Features.SingletonFeature.TestServiceTwoSingleton),
                        Flags.TestInterceptedServiceSingletonTwo
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceSingletonTwo, enableTestServiceTwo);
            }

            if (addTestServiceThree)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.SingletonFeature.ITestServiceSingleton>(
                        typeof(Features.SingletonFeature.TestServiceThreeSingleton),
                        Flags.TestInterceptedServiceSingletonThree
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceSingletonThree, enableTestServiceThree);
            }

            services.AddSingleton<ITestManagerState>(testManagerState);

            AddTestFeatureFlagManager(services);

            return implementations.ToArray();
        }

        public static FeatureFlagWrapper<Features.TransientFeature.ITestServiceTransient>[] SetTransientFeatures(
            IServiceCollection services,
            bool addTestServiceTwo = false,
            bool enableTestServiceTwo = false,
            bool addTestServiceThree = false,
            bool enableTestServiceThree = false)
        {
            var testManagerState = new TestManagerState();
            var implementations = new List<FeatureFlagWrapper<Features.TransientFeature.ITestServiceTransient>>();

            if (addTestServiceTwo)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.TransientFeature.ITestServiceTransient>(
                        typeof(Features.TransientFeature.TestServiceTwoTransient),
                        Flags.TestInterceptedServiceTransientTwo
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceTransientTwo, enableTestServiceTwo);
            }

            if (addTestServiceThree)
            {
                implementations.Add(
                    new FeatureFlagWrapper<Features.TransientFeature.ITestServiceTransient>(
                        typeof(Features.TransientFeature.TestServiceThreeTransient),
                        Flags.TestInterceptedServiceTransientThree
                    )
                );

                testManagerState.Add(Flags.TestInterceptedServiceTransientThree, enableTestServiceThree);
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