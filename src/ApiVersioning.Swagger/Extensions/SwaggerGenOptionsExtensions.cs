using Microsoft.Extensions.DependencyInjection;
using NOW.FeatureFlagExtensions.ApiVersioning.Swagger.DocumentFilters;
using NOW.FeatureFlagExtensions.ApiVersioning.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.Extensions
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void SetDefaultApiVersioningOptions(this SwaggerGenOptions swaggerGenOptions)
        {
            // Remove duplicate routes, due to /v{version:} routes on controllers.
            swaggerGenOptions.DocumentFilter<RemoveDefaultApiVersionRouteDocumentFilter>();

            // Add a custom operation filter which sets default values.
            swaggerGenOptions.OperationFilter<DefaultVersionValuesParameter>();

            // Remove required 'version' path field.
            swaggerGenOptions.OperationFilter<RemoveQueryApiVersionParamOperationFilter>();
        }
    }
}