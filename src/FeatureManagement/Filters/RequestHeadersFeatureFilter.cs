using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.FeatureManagement.Configuration;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Filters
{
    /// <summary>
    /// Can be used to only enable specific features for headers that are sent.
    /// https://github.com/microsoft/FeatureManagement-Dotnet#controllers-and-actions
    /// </summary>
    /// <example>
    /// {
    ///  "FeatureManagement": {
    ///    "BetaFeature": {
    ///      "EnabledFor": [
    ///        {
    ///          "Name": "RequestHeaders",
    ///          "Parameters": {
    ///            "RequiredHeaders": [ "BetaFeature" ]
    ///          }
    ///        }
    ///      ]
    ///    }
    ///  }
    /// </example>
    [FilterAlias(RequestHeadersFeatureFilter.FilterAlias)]
    public class RequestHeadersFeatureFilter : IFeatureFilter, IApiFeatureFilter
    {
        public const string FilterAlias = "RequestHeaders";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestHeadersFeatureFilter(IHttpContextAccessor httpContextAccessor)
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
            // Get the RequestFilterSettings from configuration.
            var settings = context.Parameters.Get<RequestFilterSettings>();
            if (settings == null || settings.RequiredHeaders == null)
            {
                return Task.FromResult(false);
            }

            return _httpContextAccessor.EvaluateHeadersAsync(settings.RequiredHeaders);
        }
    }
}