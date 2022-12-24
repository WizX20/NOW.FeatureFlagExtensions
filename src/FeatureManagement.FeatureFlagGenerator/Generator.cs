using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Extensions;
using NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Models;
using System.Diagnostics;
using System.Text;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        private const string FeatureManagementNodeName = "FeatureManagement";

        public void Execute(GeneratorExecutionContext context)
        {
            var appsettings = context.AdditionalFiles.Where(at =>
                at.Path.StartsWith("appsettings.", StringComparison.OrdinalIgnoreCase) &&
                at.Path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            ).ToList();

            if (appsettings == null || appsettings.Count < 1)
            {
                GenerateCSharpFile(context, "// Unable to generate source code: appsettings.json not found.");
                return;
            }

            var generatedContent = new StringBuilder();

            foreach (var settings in appsettings)
            {
                var sourceText = settings.GetText(context.CancellationToken);
                if (sourceText == null || sourceText.Length < 1)
                {
                    continue;
                }

                var json = sourceText.ToString();
                if (json.IndexOf($"\"{FeatureManagementNodeName}\"", StringComparison.OrdinalIgnoreCase) < 1)
                {
                    continue;
                }

                var generatedSettings = GenerateFromJsonDocument(sourceText.ToString());
                if (string.IsNullOrWhiteSpace(generatedSettings))
                {
                    continue;
                }

                generatedContent.Append(generatedSettings);
            }

            GenerateCSharpFile(context, generatedContent.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            Debug.WriteLine("Initialize code generator");
        }

        private string GenerateFromJsonDocument(string json)
        {
            var root = JsonConvert.DeserializeObject<dynamic>(json);
            if (root == null)
            {
                return string.Empty;
            }

            var featureMagementRoot = root.FeatureManagement;
            if (featureMagementRoot == null)
            {
                return string.Empty;
            }

            var featureManagement = new FeatureManagementModel
            {
                Features = DynamicExtensions.ToDictionary(featureMagementRoot)
            };

            if (featureManagement.Features == null || featureManagement.Features.Count < 1)
            {
                return string.Empty;
            }

            var generatedContent = new StringBuilder();
            foreach (var feature in featureManagement.Features)
            {
                generatedContent.AppendLine(string.Format(Constants.FeatureFlagFormat, feature.Key));
            }

            return generatedContent.ToString();
        }

        private void GenerateCSharpFile(GeneratorExecutionContext context, string content, string fileName = "GeneratedFeatureFlags.g.cs")
        {
            var fileContent = Constants.ClassContent.Replace(Constants.ReleaseFlagsKey, content);
            var sourceText = SourceText.From(fileContent, Encoding.UTF8);
            context.AddSource(fileName, sourceText);
        }
    }
}