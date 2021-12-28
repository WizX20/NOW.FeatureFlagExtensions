using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NOW.FeatureFlagExtensions.ApiVersioning.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly ApiVersioningOptions _apiVersioningOptions;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            ApiVersioningOptions apiVersioningOptions)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _provider = provider;
            _apiVersioningOptions = apiVersioningOptions;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Add a swagger document for each discovered API version.
            // Note: We might choose to skip or document deprecated API versions differently.
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var apiDescription = _apiVersioningOptions.ApiDescription;

            var info = new OpenApiInfo()
            {
                Title = apiDescription?.Title,
                Description = apiDescription?.Description,
                Version = description.ApiVersion.ToString()
            };

            if (apiDescription?.Contact != null)
            {
                info.Contact = new OpenApiContact()
                {
                    Name = apiDescription.Contact.Name,
                    Email = apiDescription.Contact.Email
                };
            }

            if (apiDescription?.License != null)
            {
                var uri = new Uri("https://unlicense.org/");
                if (!string.IsNullOrWhiteSpace(apiDescription.License.Url))
                {
                    uri = new Uri(apiDescription.License.Url);
                }

                info.License = new OpenApiLicense()
                {
                    Name = apiDescription.License.Name,
                    Url = uri
                };
            }

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}