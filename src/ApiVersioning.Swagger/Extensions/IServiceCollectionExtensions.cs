using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NOW.FeatureFlagExtensions.ApiVersioning.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureApiVersioningSwaggerGenOptions(this IServiceCollection services, ApiVersioningOptions? apiVersioningOptions = null)
        {
            if (apiVersioningOptions != null)
            {
                services.TryAddSingleton(apiVersioningOptions);
            }

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}