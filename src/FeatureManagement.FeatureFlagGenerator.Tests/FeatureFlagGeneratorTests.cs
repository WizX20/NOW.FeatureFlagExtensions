#pragma warning disable RCS1196 // Call extension method as instance method: We want to be explicit with extensions.

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Text;
using VerifyCS = NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Tests.CSharpSourceGeneratorVerifier
    <NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Generator>;
using Microsoft.CodeAnalysis.Testing;
using System.IO;

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
            var code = "initial code";
            var generated = "expected generated code";

            // Act
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            //    .Where(a =>
            //        !a.IsDynamic &&
            //        !string.IsNullOrWhiteSpace(a.Location) &&
            //        a.FullName.StartsWith("NOW.")
            //    ).ToList();

            //var bla = GetGeneratedOutput(code);

            var appsettingsContent = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + _appsettings_json_filename);
            await new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { code },
                    AdditionalFiles = { (_appsettings_json_filename, appsettingsContent) },
                    GeneratedSources =
                    {
                        (typeof(Generator), "GeneratedFileName", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                    },
                },
            }.RunAsync();

            // Assert
        }

        private string GetGeneratedOutput(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new List<MetadataReference>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                {
                    references.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            var compilation = CSharpCompilation.Create("foo", new SyntaxTree[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // TODO: Uncomment this line if you want to fail tests when the injected program isn't valid _before_ running generators
            // var compileDiagnostics = compilation.GetDiagnostics();
            // Assert.False(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

            ISourceGenerator generator = new Generator();

            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);
            //Assert.False(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

            return outputCompilation.SyntaxTrees.Last().ToString();
        }
    }
}