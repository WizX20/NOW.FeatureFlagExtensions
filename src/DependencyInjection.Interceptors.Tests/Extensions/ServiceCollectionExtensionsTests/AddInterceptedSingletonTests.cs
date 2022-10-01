#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.InterceptorInjection.SingletonFeature;
using Flags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.InterceptorInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Tests.Extensions.ServiceCollectionExtensionsTests
{
    [TestClass]
    public class AddInterceptedSingletonTests
    {
        [TestMethod]
        public void AddInterceptedSingleton_ServicesArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection? services = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(services));
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            Models.FeatureFlagWrapper<Features.ITestServiceSingleton>[]? implementations = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsArgumentEmpty_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();
            var implementations = Array.Empty<FeatureFlagWrapper<Features.ITestServiceSingleton>>();

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationWithNoFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceSingleton>[] {
                new FeatureFlagWrapper<Features.ITestServiceSingleton>(
                    typeof(Features.TestServiceTwoSingleton),
                    Flags.TestInterceptedServiceFake // non-existent
                )
            };

            var testManagerState = new TestManagerState();
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneSingleton>(services);
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationWithDisabledFlagSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetSingletonFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneSingleton>(services);
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsWithDisabledFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetSingletonFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneSingleton>(services);
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsWithFirstFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetSingletonFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceTwoSingleton>(services);
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsWithSecondFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetSingletonFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeSingleton>(services);
        }

        [TestMethod]
        public void AddInterceptedSingleton_ImplementationsWithBothFlagsSet_ShouldReturnLastFoundEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetSingletonFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedSingleton<Features.ITestServiceSingleton, Features.TestServiceOneSingleton>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeSingleton>(services);
        }

        private void AssertEnabledService<TImplementation>(IServiceCollection services)
            where TImplementation : class
        {
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService<Features.ITestServiceSingleton>();
            extractedService.Should().NotBeNull();

            var testServiceScoped = serviceProvider.GetService<TImplementation>() as Features.ITestServiceSingleton;
            testServiceScoped.Should().NotBeNull();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            extractedService.Name.Should().Be(testServiceScoped.Name);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}