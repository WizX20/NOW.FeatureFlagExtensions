using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FeatureTestApplication.Swagger.OperationFilters
{
    public class ValidateRequiredParameters : IOperationFilter
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

            var baseMethod = context.MethodInfo.GetBaseDefinition();
            if (baseMethod == null)
            {
                return;
            }

            foreach (var baseParameter in baseMethod.GetParameters())
            {
                if (baseParameter.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    var fromQueryName = baseParameter.GetCustomAttribute<FromQueryAttribute>()?.Name;
                    if (fromQueryName == null)
                    {
                        continue;
                    }

                    var parameter = operation.Parameters.Single(x => x.Name == baseParameter.Name || x.Name == fromQueryName);
                    parameter.Required = true;
                }
            }
        }
    }
}