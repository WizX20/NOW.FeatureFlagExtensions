using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureApiVersioningSwaggerGenOptions(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}