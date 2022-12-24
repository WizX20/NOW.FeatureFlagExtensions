#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.DefaultInjection.TransientFeature;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Tests.Extensions.ServiceCollectionExtensionsTests
{
    [TestClass]
    public class AddTransientTests
    {
        [TestMethod]
        public void AddTransient_ServicesArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection? services = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(services));
        }

        [TestMethod]
        public void AddTransient_ImplementationsArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            Models.FeatureFlagWrapper<Features.ITestServiceTransient>[]? implementations = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddTransient_ImplementationsArgumentEmpty_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();
            var implementations = Array.Empty<Models.FeatureFlagWrapper<Features.ITestServiceTransient>>();

            // Act
            var act = () => ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddTransient_ImplementationWithNoFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceTransient>[] {
                new FeatureFlagWrapper<Features.ITestServiceTransient>(
                    typeof(Features.TestServiceTwoTransient),
                    "TestDefaultServiceFake" // non-existent
                )
            };

            var testManagerState = new TestManagerState();
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneTransient>();
        }

        [TestMethod]
        public void AddTransient_ImplementationWithDisabledFlagSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = ServiceCollectionExtensionsFixture.SetTransientFeatures(
                services: services,
                addTestServiceTwo: true,
                enableTestServiceTwo: false
            );

            // Act
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneTransient>();
        }

        [TestMethod]
        public void AddTransient_ImplementationsWithDisabledFlagsSet_ShouldReturnDefaultService()
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
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneTransient>();
        }

        [TestMethod]
        public void AddTransient_ImplementationsWithFirstFlagSet_ShouldReturnEnabledService()
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
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceTwoTransient>();
        }

        [TestMethod]
        public void AddTransient_ImplementationsWithSecondFlagSet_ShouldReturnEnabledService()
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
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceThreeTransient>();
        }

        [TestMethod]
        public void AddTransient_ImplementationsWithBothFlagsSet_ShouldReturnFirstFoundEnabledService()
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
            ServiceCollectionExtensions.AddTransient<Features.ITestServiceTransient, Features.TestServiceOneTransient>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceTransient));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceTwoTransient>();
        }
    }
}