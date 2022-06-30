#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;
using System.Reflection;
using DefaultInjectionFlags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.DefaultInjection;
using DefaultInjectionTests = TestFeatures.DefaultInjection;

namespace FeatureTestBenchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class DefaultInjectionBenchmarks
    {
        private IFeatureFlagManager _featureFlagManager;
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = GetConfiguration();
            serviceCollection
                .AddSingleton<IConfiguration>(configuration)
                .AddFeatureManagement();

            serviceCollection
                .AddFeatureFlagFeatureManagementManager();

            serviceCollection
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceOneScoped>()
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceTwoScoped>()
                .AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceThreeScoped>();

            serviceCollection
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceOneTransient>()
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceTwoTransient>()
                .AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceThreeTransient>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
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

        private IConfiguration GetConfiguration()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var applicationPath = Path.GetDirectoryName(assemblyPath);

            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(applicationPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            return configuration;
        }
    }
}