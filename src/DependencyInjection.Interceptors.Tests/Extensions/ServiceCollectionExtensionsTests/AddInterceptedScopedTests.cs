#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.InterceptorInjection.ScopedFeature;
using Flags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.InterceptorInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Tests.Extensions.ServiceCollectionExtensionsTests
{
    [TestClass]
    public class AddInterceptedScopedTests
    {
        [TestMethod]
        public void AddInterceptedScoped_ServicesArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection? services = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(services));
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            Models.FeatureFlagWrapper<Features.ITestServiceScoped>[]? implementations = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsArgumentEmpty_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();
            var implementations = Array.Empty<FeatureFlagWrapper<Features.ITestServiceScoped>>();

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationWithNoFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestInterceptedServiceFake // non-existent
                )
            };

            var testManagerState = new TestManagerState();
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneScoped>(services);
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationWithDisabledFlagSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetScopedFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneScoped>(services);
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsWithDisabledFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetScopedFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneScoped>(services);
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsWithFirstFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetScopedFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceTwoScoped>(services);
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsWithSecondFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetScopedFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeScoped>(services);
        }

        [TestMethod]
        public void AddInterceptedScoped_ImplementationsWithBothFlagsSet_ShouldReturnLastFoundEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetScopedFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeScoped>(services);
        }

        private void AssertEnabledService<TImplementation>(IServiceCollection services)
            where TImplementation : class
        {
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService<Features.ITestServiceScoped>();
            extractedService.Should().NotBeNull();

            var testServiceScoped = serviceProvider.GetService<TImplementation>() as Features.ITestServiceScoped;
            testServiceScoped.Should().NotBeNull();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            extractedService.Name.Should().Be(testServiceScoped.Name);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}