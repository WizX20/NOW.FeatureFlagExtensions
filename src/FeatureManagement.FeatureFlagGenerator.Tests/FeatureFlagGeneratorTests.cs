using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Tests.TestConfigurations.AppSettings;
using System.Text;
using GeneratedCodeVerifier = NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Tests.CSharpSourceGeneratorVerifier
    <NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Generator>;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Tests
{
    [TestClass]
    public class FeatureFlagGeneratorTests
    {
        private const string _appsettings_json_filename = "appsettings.json";

        [TestMethod]
        public async Task Execute_WithoutAdditionalFiles_ShouldGenerateMessage()
        {
            // Arrange
            var generated = Expected.GeneratedCode;
            var appsettingsContent = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "TestConfigurations\\AppSettings\\" + _appsettings_json_filename);

            // Act
            var verifier = new GeneratedCodeVerifier.Test
            {
                TestState =
                {
                    AdditionalFiles = { (_appsettings_json_filename, appsettingsContent) },
                    GeneratedSources =
                    {
                        (typeof(Generator), "GeneratedFeatureFlags.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                },
            };

            // Assert
            await verifier.RunAsync();
        }
    }
}