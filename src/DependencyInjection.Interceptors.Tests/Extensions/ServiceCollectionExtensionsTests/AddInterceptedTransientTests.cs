#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.InterceptorInjection.TransientFeature;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Tests.Extensions.ServiceCollectionExtensionsTests
{
    [TestClass]
    public class AddInterceptedTransientTests
    {
        [TestMethod]
        public void AddInterceptedTransient_ServicesArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection? services = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(services));
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            Models.FeatureFlagWrapper<Features.ITestServiceTransient>[]? implementations = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsArgumentEmpty_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();
            var implementations = Array.Empty<FeatureFlagWrapper<Features.ITestServiceTransient>>();

            // Act
            var act = () => ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationWithNoFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceTransient>[] {
                new FeatureFlagWrapper<Features.ITestServiceTransient>(
                    typeof(Features.TestServiceTwoTransient),
                    "TestInterceptedServiceFake" // non-existent
                )
            };

            var testManagerState = new TestManagerState();
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneTransient>(services);
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationWithDisabledFlagSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneTransient>(services);
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsWithDisabledFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceOneTransient>(services);
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsWithFirstFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: false
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceTwoTransient>(services);
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsWithSecondFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeTransient>(services);
        }

        [TestMethod]
        public void AddInterceptedTransient_ImplementationsWithBothFlagsSet_ShouldReturnLastFoundEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: true,
                addTestServiceThree: true,
                enableTestServiceThree: true
            );

            // Act
            ServiceCollectionExtensions.AddInterceptedTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            AssertEnabledService<Features.TestServiceThreeTransient>(services);
        }

        private void AssertEnabledService<TImplementation>(IServiceCollection services)
            where TImplementation : class
        {
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService<Features.ITestServiceTransient>();
            extractedService.Should().NotBeNull();

            var testServiceScoped = serviceProvider.GetService<TImplementation>() as Features.ITestServiceTransient;
            testServiceScoped.Should().NotBeNull();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            extractedService.Name.Should().Be(testServiceScoped.Name);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}