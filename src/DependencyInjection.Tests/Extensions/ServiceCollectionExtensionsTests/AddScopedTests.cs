#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using TestFeatureFlagManager;
using Features = TestFeatures.DefaultInjection.ScopedFeature;
using Flags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.DefaultInjection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Tests.Extensions.ServiceCollectionExtensionsTests
{
    [TestClass]
    public class AddScopedTests
    {
        [TestMethod]
        public void AddScoped_ServicesArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection? services = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(services));
        }

        [TestMethod]
        public void AddScoped_ImplementationsArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            Models.FeatureFlagWrapper<Features.ITestServiceScoped>[]? implementations = null;

            // Act
            var act = () => ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddScoped_ImplementationsArgumentEmpty_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();
            var implementations = Array.Empty<Models.FeatureFlagWrapper<Features.ITestServiceScoped>>();

            // Act
            var act = () => ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName(nameof(implementations));
        }

        [TestMethod]
        public void AddScoped_ImplementationWithNoFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceFake // non-existent
                )
            };

            var testManagerState = new TestManagerState();
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneScoped>();
        }

        [TestMethod]
        public void AddScoped_ImplementationWithDisabledFlagSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo
                )
            };

            var testManagerState = new TestManagerState();
            testManagerState.Add(Flags.TestDefaultServiceScopedTwo, false);
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneScoped>();
        }

        [TestMethod]
        public void AddScoped_ImplementationsWithDisabledFlagsSet_ShouldReturnDefaultService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo
                ),
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceThreeScoped),
                    Flags.TestDefaultServiceScopedThree
                )
            };

            var testManagerState = new TestManagerState();
            testManagerState.Add(Flags.TestDefaultServiceScopedTwo, false);
            testManagerState.Add(Flags.TestDefaultServiceScopedThree, false);
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceOneScoped>();
        }

        [TestMethod]
        public void AddScoped_ImplementationsWithFirstFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo
                ),
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceThreeScoped),
                    Flags.TestDefaultServiceScopedThree
                )
            };

            var testManagerState = new TestManagerState();
            testManagerState.Add(Flags.TestDefaultServiceScopedTwo, true);
            testManagerState.Add(Flags.TestDefaultServiceScopedThree, false);
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceTwoScoped>();
        }

        [TestMethod]
        public void AddScoped_ImplementationsWithSecondFlagSet_ShouldReturnEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo
                ),
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceThreeScoped),
                    Flags.TestDefaultServiceScopedThree
                )
            };

            var testManagerState = new TestManagerState();
            testManagerState.Add(Flags.TestDefaultServiceScopedTwo, false);
            testManagerState.Add(Flags.TestDefaultServiceScopedThree, true);
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceThreeScoped>();
        }

        [TestMethod]
        public void AddScoped_ImplementationsWithBothFlagsSet_ShouldReturnFirstFoundEnabledService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            var implementations = new FeatureFlagWrapper<Features.ITestServiceScoped>[] {
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceTwoScoped),
                    Flags.TestDefaultServiceScopedTwo
                ),
                new FeatureFlagWrapper<Features.ITestServiceScoped>(
                    typeof(Features.TestServiceThreeScoped),
                    Flags.TestDefaultServiceScopedThree
                )
            };

            var testManagerState = new TestManagerState();
            testManagerState.Add(Flags.TestDefaultServiceScopedTwo, true);
            testManagerState.Add(Flags.TestDefaultServiceScopedThree, true);
            services.AddSingleton<ITestManagerState>(testManagerState);

            _ = ServiceCollectionExtensionsFixture.AddTestFeatureFlagManager(services);

            // Act
            ServiceCollectionExtensions.AddScoped<Features.ITestServiceScoped, Features.TestServiceOneScoped>(
                services,
                implementations
            );

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var extractedService = serviceProvider.GetService(typeof(Features.ITestServiceScoped));
            extractedService.Should().NotBeNull();
            extractedService.Should().BeOfType<Features.TestServiceTwoScoped>();
        }
    }
}