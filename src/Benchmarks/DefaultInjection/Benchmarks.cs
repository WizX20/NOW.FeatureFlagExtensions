#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.Benchmarks.Common.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using DefaultInjectionTests = TestFeatures.DefaultInjection;
using Flags = TestFeatures.FeatureFlagsGenerated.ReleaseFlags;

namespace NOW.FeatureFlagExtensions.Benchmarks.DefaultInjection
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

            services.AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceOneScoped>(
                new FeatureFlagWrapper<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(DefaultInjectionTests.ScopedFeature.TestServiceThreeScoped),
                    Flags.TestDefaultServiceScopedThree),
                new FeatureFlagWrapper<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(DefaultInjectionTests.ScopedFeature.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo),
                new FeatureFlagWrapper<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>(
                    typeof(DefaultInjectionTests.ScopedFeature.TestServiceOneScoped),
                    "TestDefaultServiceScopedOne")
            );

            services.AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceOneTransient>(
                new FeatureFlagWrapper<DefaultInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(DefaultInjectionTests.TransientFeature.TestServiceThreeTransient),
                    Flags.TestDefaultServiceTransientThree),
                new FeatureFlagWrapper<DefaultInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(DefaultInjectionTests.TransientFeature.TestServiceTwoTransient),
                    Flags.TestDefaultServiceTransientTwo),
                new FeatureFlagWrapper<DefaultInjectionTests.TransientFeature.ITestServiceTransient>(
                    typeof(DefaultInjectionTests.TransientFeature.TestServiceOneTransient),
                    "TestDefaultServiceTransientOne")
            );

            _serviceProvider = services.BuildServiceProvider();
        }

        [Benchmark]
        public void GetTestServiceScoped()
        {
            var resolvedService = _serviceProvider.GetService<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>();
            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }

        [Benchmark]
        public void GetTestServiceTransient()
        {
            var resolvedService = _serviceProvider.GetService<DefaultInjectionTests.TransientFeature.ITestServiceTransient>();
            if (resolvedService == null)
            {
                throw new ArgumentNullException(nameof(resolvedService));
            }
        }
    }
}