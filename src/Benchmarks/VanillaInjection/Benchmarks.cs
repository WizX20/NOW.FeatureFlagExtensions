#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.Benchmarks.Common.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using DefaultInjectionFlags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.DefaultInjection;
using DefaultInjectionTests = TestFeatures.DefaultInjection;

namespace NOW.FeatureFlagExtensions.Benchmarks.VanillaInjection
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmarks
    {
        private IFeatureFlagManager _featureFlagManager;
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

            services
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceOneScoped>()
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceTwoScoped>()
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceThreeScoped>();

            services
                .AddScoped<DefaultInjectionTests.SingletonFeature.ITestServiceSingleton, DefaultInjectionTests.SingletonFeature.TestServiceOneSingleton>()
                .AddScoped<DefaultInjectionTests.SingletonFeature.ITestServiceSingleton, DefaultInjectionTests.SingletonFeature.TestServiceTwoSingleton>()
                .AddScoped<DefaultInjectionTests.SingletonFeature.ITestServiceSingleton, DefaultInjectionTests.SingletonFeature.TestServiceThreeSingleton>();

            services
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceOneTransient>()
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceTwoTransient>()
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceThreeTransient>();

            _serviceProvider = services.BuildServiceProvider();
            _featureFlagManager = _serviceProvider.GetRequiredService<IFeatureFlagManager>();
        }

        [Benchmark]
        public void GetTestServiceScoped()
        {
            var resolvedService = _serviceProvider.GetService<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>();

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceScopedThree))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.ScopedFeature.TestServiceThreeScoped>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceScopedTwo))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.ScopedFeature.TestServiceTwoScoped>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceScopedOne))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.ScopedFeature.TestServiceOneScoped>();
            }

            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }

        [Benchmark]
        public void GetTestServiceSingleton()
        {
            var resolvedService = _serviceProvider.GetService<DefaultInjectionTests.SingletonFeature.ITestServiceSingleton>();

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceSingletonThree))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.SingletonFeature.TestServiceThreeSingleton>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceSingletonTwo))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.SingletonFeature.TestServiceTwoSingleton>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceSingletonOne))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.SingletonFeature.TestServiceOneSingleton>();
            }

            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }

        [Benchmark]
        public void GetTestServiceTransient()
        {
            var resolvedService = _serviceProvider.GetService<DefaultInjectionTests.TransientFeature.ITestServiceTransient>();

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceTransientThree))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.TransientFeature.TestServiceThreeTransient>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceTransientTwo))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.TransientFeature.TestServiceTwoTransient>();
            }

            if (_featureFlagManager.IsEnabled(DefaultInjectionFlags.TestDefaultServiceTransientOne))
            {
                resolvedService = _serviceProvider.GetService<DefaultInjectionTests.TransientFeature.TestServiceOneTransient>();
            }

            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }
    }
}