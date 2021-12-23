using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.FeatureManagement.Configuration;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Filters
{
    /// <summary>
    /// Can be used to only enable specific features for claims the user has.
    /// </summary>
    /// <example>
    /// {
    ///  "FeatureManagement": {
    ///    "BetaFeature": {
    ///      "EnabledFor": [
    ///        {
    ///          "Name": "Claims",
    ///          "Parameters": {
    ///            "RequiredClaims": [ "Internal" ]
    ///          }
    ///        }
    ///      ]
    ///    }
    ///  }
    /// </example>
    [FilterAlias(ClaimsFeatureFilter.FilterAlias)]
    public class ClaimsFeatureFilter : IFeatureFilter, IApiFeatureFilter
    {
        public const string FilterAlias = "Claims";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _httpContextAccessor = httpContextAccessor;
        }

        public string GetFilterAlias()
        {
            return FilterAlias;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // Get the ClaimsFilterSettings from configuration.
            var settings = context.Parameters.Get<ClaimsFilterSettings>();
            if (settings == null || settings.RequiredClaims == null)
            {
                return Task.FromResult(false);
            }

            return _httpContextAccessor.EvaluateClaimsAsync(settings.RequiredClaims);
        }
    }
}