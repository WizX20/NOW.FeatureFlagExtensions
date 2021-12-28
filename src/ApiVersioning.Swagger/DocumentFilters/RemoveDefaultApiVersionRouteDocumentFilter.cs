using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.DocumentFilters
{
    public class RemoveDefaultApiVersionRouteDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc is null)
            {
                throw new ArgumentNullException(nameof(swaggerDoc));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var apiDescription in context.ApiDescriptions)
            {
                var versionParam = apiDescription.ParameterDescriptions
                    .FirstOrDefault(p =>
                        p.Name == "version" &&
                        p.Source.Id.Equals("Path", StringComparison.InvariantCultureIgnoreCase)
                    );

                if (versionParam == null)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(apiDescription.RelativePath))
                {
                    var route = "/" + apiDescription.RelativePath.TrimEnd('/');
                    swaggerDoc.Paths.Remove(route);
                }
            }
        }
    }
}