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
            var appsettings = GetAppsettingsFiles(context.AdditionalFiles)?.ToList();
            if (appsettings == null || appsettings.Count < 1)
            {
                GenerateCSharpFile(context, "// Unable to generate feature flags: appsettings.json not found.");
                return;
            }

            try
            {
                var generatedContent = new StringBuilder();

                foreach (var settings in appsettings)
                {
                    var jsonContent = GetJsonContent(settings, context.CancellationToken);
                    var generatedSettings = GenerateFromJsonDocument(jsonContent);
                    if (string.IsNullOrWhiteSpace(generatedSettings))
                    {
                        continue;
                    }

                    generatedContent.Append(generatedSettings);
                }

                GenerateCSharpFile(context, generatedContent.ToString());
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            Debug.WriteLine("Initialize code generator");

            // Enable for debugging on build.
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
        }

        private IEnumerable<AdditionalText>? GetAppsettingsFiles(IReadOnlyList<AdditionalText> additionalFiles)
        {
            if (additionalFiles == null || additionalFiles.Count < 1)
            {
                return Enumerable.Empty<AdditionalText>();
            }

            return additionalFiles.Where(af => IsValidAppsettingsFile(af));
        }

        private bool IsValidAppsettingsFile(AdditionalText additionalFile)
        {
            var fileName = additionalFile.Path.Split('\\')?.LastOrDefault();
            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            if (!fileName.StartsWith("appsettings.", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private string GetJsonContent(AdditionalText appsettings, CancellationToken cancellationToken)
        {
            var sourceText = appsettings.GetText(cancellationToken);
            if (sourceText == null || sourceText.Length < 1)
            {
                return string.Empty;
            }

            var json = sourceText.ToString();
            if (json.IndexOf($"\"{FeatureManagementNodeName}\"", StringComparison.OrdinalIgnoreCase) < 1)
            {
                return string.Empty;
            }

            return json;
        }

        private string GenerateFromJsonDocument(string json)
        {
            var root = JsonConvert.DeserializeObject<dynamic>(json);
            var featureManagementRoot = root!.FeatureManagement;
            var featureManagement = new FeatureManagementModel
            {
                Features = DynamicExtensions.ToDictionary(featureManagementRoot)
            };

            if (featureManagement.Features == null || featureManagement.Features.Count < 1)
            {
                return string.Empty;
            }

            var generatedContent = new StringBuilder();
            foreach (var feature in featureManagement.Features)
            {
                generatedContent.AppendFormat(Constants.FeatureFlagFormat, feature.Key).AppendLine();
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