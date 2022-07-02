#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.Benchmarks.Common.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using InterceptorInjectionFlags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.InterceptorInjection;
using InterceptorInjectionTests = TestFeatures.InterceptorInjection;

namespace NOW.FeatureFlagExtensions.Benchmarks.InterceptorInjection
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmarks
    {
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            services
                .AddConfiguration()
                .AddFeatureManagement();

            services
                .AddFeatureFlagFeatureManagementManager();

            services.AddInterceptedScoped<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped, InterceptorInjectionTests.ScopedFeature.TestServiceOneScoped>(
                new FeatureFlagWrapper<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(InterceptorInjectionTests.ScopedFeature.TestServiceThreeScoped),
                    InterceptorInjectionFlags.TestInterceptedServiceScopedThree),
                new FeatureFlagWrapper<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(InterceptorInjectionTests.ScopedFeature.TestServiceTwoScoped),
                    InterceptorInjectionFlags.TestInterceptedServiceScopedTwo),
                new FeatureFlagWrapper<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(InterceptorInjectionTests.ScopedFeature.TestServiceOneScoped),
                    InterceptorInjectionFlags.TestInterceptedServiceScopedOne)
            );

            services.AddInterceptedSingleton<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton, InterceptorInjectionTests.SingletonFeature.TestServiceOneSingleton>(
                new FeatureFlagWrapper<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>(
                    typeof(InterceptorInjectionTests.SingletonFeature.TestServiceThreeSingleton),
                    InterceptorInjectionFlags.TestInterceptedServiceSingletonThree),
                new FeatureFlagWrapper<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>(
                    typeof(InterceptorInjectionTests.SingletonFeature.TestServiceTwoSingleton),
                    InterceptorInjectionFlags.TestInterceptedServiceSingletonTwo),
                new FeatureFlagWrapper<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>(
                    typeof(InterceptorInjectionTests.SingletonFeature.TestServiceOneSingleton),
                    InterceptorInjectionFlags.TestInterceptedServiceSingletonOne)
            );

            services.AddInterceptedTransient<InterceptorInjectionTests.TransientFeature.ITestServiceTransient, InterceptorInjectionTests.TransientFeature.TestServiceOneTransient>(
                new FeatureFlagWrapper<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(InterceptorInjectionTests.TransientFeature.TestServiceThreeTransient),
                    InterceptorInjectionFlags.TestInterceptedServiceTransientThree),
                new FeatureFlagWrapper<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(InterceptorInjectionTests.TransientFeature.TestServiceTwoTransient),
                    InterceptorInjectionFlags.TestInterceptedServiceTransientTwo),
                new FeatureFlagWrapper<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(InterceptorInjectionTests.TransientFeature.TestServiceOneTransient),
                    InterceptorInjectionFlags.TestInterceptedServiceTransientOne)
            );

            _serviceProvider = services.BuildServiceProvider();
        }

        [Benchmark]
        public void GetTestServiceScoped()
        {
            var resolvedService = _serviceProvider.GetService<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>();
            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }

        [Benchmark]
        public void GetTestServiceSingleton()
        {
            var resolvedService = _serviceProvider.GetService<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>();
            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }

        [Benchmark]
        public void GetTestServiceTransient()
        {
            var resolvedService = _serviceProvider.GetService<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>();
            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }
    }
}