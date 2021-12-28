using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Extensions
{
    public static class ApiVersioningOptionsExtensions
    {
        public static void SetDefaultApiVersioningOptions(
            this ApiVersioningOptions apiVersioningOptions,
            ApiVersion defaultApiVersion)
        {
            if (apiVersioningOptions is null)
            {
                throw new ArgumentNullException(nameof(apiVersioningOptions));
            }

            if (defaultApiVersion is null)
            {
                throw new ArgumentNullException(nameof(defaultApiVersion));
            }

            apiVersioningOptions.DefaultApiVersion = defaultApiVersion;
            apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningOptions.ReportApiVersions = true;
            apiVersioningOptions.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader(Constants.HttpHeaders.ApiVersion),
                new HeaderApiVersionReader(Constants.HttpHeaders.ApiVersion)
            );
        }
    }
}