using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.OperationFilters
{
    public class RemoveQueryApiVersionParamOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            RemoveOperationParameter(operation, "version");
        }

        private void RemoveOperationParameter(OpenApiOperation operation, string name)
        {
            var versionParameter = operation.Parameters
                .FirstOrDefault(p => p.Name == name && p.In == ParameterLocation.Query);

            if (versionParameter != null)
            {
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}