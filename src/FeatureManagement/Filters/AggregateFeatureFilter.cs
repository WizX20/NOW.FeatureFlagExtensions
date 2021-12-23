using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.FeatureManagement.Configuration;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Filters
{
    /// <summary>
    /// Can be used to only enable specific features for all available filters combined (all must be true).
    /// https://github.com/microsoft/FeatureManagement-Dotnet#controllers-and-actions
    /// </summary>
    /// <example>
    /// {
    ///  "FeatureManagement": {
    ///    "BetaFeature": {
    ///      "EnabledFor": [
    ///        {
    ///          "Name": "Combined",
    ///          "Parameters": {
    ///            "Environments": [ "Local", "Development", "Staging", "Production" ],
    ///            "RequiredClaims": [ "Internal" ],
    ///            "RequiredHeaders": [ "BetaFeature" ]
    ///          }
    ///        }
    ///      ]
    ///    }
    ///  }
    /// </example>
    [FilterAlias(AggregateFeatureFilter.FilterAlias)]
    public class AggregateFeatureFilter : IFeatureFilter, IApiFeatureFilter
    {
        public const string FilterAlias = "Combined";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public AggregateFeatureFilter(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            if (environment is null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public string GetFilterAlias()
        {
            return FilterAlias;
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // Get the CombinedFilterSettings from configuration.
            var settings = context.Parameters.Get<AggregateFilterSettings>();

            if (settings == null)
            {
                return false;
            }

            // Check all available filters.
            var isEnabled = await _environment.EvaluateEnvironmentAsync(settings.Environments);
            if (!isEnabled)
            {
                return false;
            }

            isEnabled = await _httpContextAccessor.EvaluateClaimsAsync(settings.RequiredClaims);
            if (!isEnabled)
            {
                return false;
            }

            isEnabled = await _httpContextAccessor.EvaluateHeadersAsync(settings.RequiredHeaders);
            if (!isEnabled)
            {
                return false;
            }

            return true;
        }
    }
}