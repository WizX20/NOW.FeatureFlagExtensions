using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger.OperationFilters
{
    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the default API version parameter.
    /// </summary>
    public class DefaultVersionValuesParameter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
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

            if (operation.Parameters == null)
            {
                return;
            }

            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (parameter.Description == null)
                {
                    if (!string.IsNullOrEmpty(description.ModelMetadata?.Description))
                    {
                        parameter.Description = description.ModelMetadata?.Description;
                    }

                    if (parameter.Name.Equals(Constants.HttpHeaders.ApiVersion, StringComparison.OrdinalIgnoreCase))
                    {
                        parameter.Description = string.IsNullOrEmpty(parameter.Description) ? string.Empty : parameter.Description += ". ";
                        parameter.Description += "If no version is given, the api will fall back on the default version. " +
                            "If both the header and query are used, the api will prefer the query (path) above the header value. " +
                            "Additionally you can define the version in the path itself, for example: 'api/1/action', 'api/v1.0/action'";
                    }
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}