using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NOW.FeatureFlagExtensions.FeatureManagement.Configuration;
using NOW.FeatureFlagExtensions.FeatureManagement.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Swagger.OperationFilters
{
    public class FeatureFilterHeaderParameter : IOperationFilter
    {
        private readonly IFeatureDefinitionProvider _featureDefinitionProvider;

        public FeatureFilterHeaderParameter(IFeatureDefinitionProvider featureDefinitionProvider)
        {
            if (featureDefinitionProvider is null)
            {
                throw new ArgumentNullException(nameof(featureDefinitionProvider));
            }

            _featureDefinitionProvider = featureDefinitionProvider;
        }

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
                operation.Parameters = new List<OpenApiParameter>();
            }

            // Get all feature switches defined in config.
            var features = _featureDefinitionProvider.GetAllFeatureDefinitionsAsync()
                .ToListAsync()
                .AsTask()
                .Result;

            if (features == null || features.Count < 1)
            {
                return;
            }

            // Get all possible header values that trigger features.
            var headers = new List<string>();
            foreach (var feature in features)
            {
                var filters = feature.EnabledFor.Where(a =>
                    a.Name.Equals(AggregateFeatureFilter.FilterAlias) ||
                    a.Name.Equals(RequestHeadersFeatureFilter.FilterAlias)
                );

                foreach (var filter in filters)
                {
                    var headersSection = filter.Parameters.GetSection(nameof(RequestFilterSettings.RequiredHeaders));
                    if (headersSection == null)
                    {
                        continue;
                    }

                    var headerValues = headersSection.Get<string[]>();
                    if (headerValues == null || headerValues.Length < 1)
                    {
                        continue;
                    }

                    headers.AddRange(headerValues);
                }
            }

            foreach (var header in headers)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = header,
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(false)
                    }
                });
            }
        }
    }
}