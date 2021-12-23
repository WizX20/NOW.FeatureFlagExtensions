using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.FeatureManagement.Configuration;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Filters
{
    /// <summary>
    /// Can be used to only enable specific features for one or more environments specified.
    /// https://github.com/microsoft/FeatureManagement-Dotnet#controllers-and-actions
    /// </summary>
    /// <example>
    /// {
    ///  "FeatureManagement": {
    ///    "BetaFeature": {
    ///      "EnabledFor": [
    ///        {
    ///          "Name": "Environments",
    ///          "Parameters": {
    ///            "Environments": [ "Local", "Development", "Staging", "Production" ]
    ///          }
    ///        }
    ///      ]
    ///    }
    ///  }
    /// </example>
    [FilterAlias(EnvironementsFeatureFilter.FilterAlias)]
    public class EnvironementsFeatureFilter : IFeatureFilter, IApiFeatureFilter
    {
        public const string FilterAlias = "Environments";

        private readonly IWebHostEnvironment _environment;

        public EnvironementsFeatureFilter(IWebHostEnvironment environment)
        {
            if (environment is null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _environment = environment;
        }

        public string GetFilterAlias()
        {
            return FilterAlias;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // Get the RequestFilterSettings from configuration.
            var settings = context.Parameters.Get<EnvironmentFilterSettings>();
            if (settings == null || settings.Environments == null)
            {
                return Task.FromResult(false);
            }

            return _environment.EvaluateEnvironmentAsync(settings.Environments);
        }
    }
}