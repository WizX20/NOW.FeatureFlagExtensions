using FeatureTestApplication.Mvc.ExceptionFilterAttributes;
using Microsoft.AspNetCore.Mvc;

namespace FeatureTestApplication.Extensions.ServiceCollection
{
    public static class MvcExtensions
    {
        /// <summary>
        /// Adds default MVC options by configuring the <see cref="Constants.ResponseCache.DefaultCacheProfile"/>
        /// and adds <see cref="Newtonsoft.Json"/> as default serializer.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddDefaultMvcOptions(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services
                .AddControllers(
                    options =>
                    {
                        // Refer to this article for more details on how to properly set the caching for your needs:
                        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response
                        options.CacheProfiles.Add(
                                Constants.ResponseCache.DefaultCacheProfile,
                                new CacheProfile
                                {
                                    Duration = 600, // seconds.
                                    Location = ResponseCacheLocation.Any,
                                    VaryByHeader = "*"
                                });

                        // Handle canceled (async) requests.
                        options.Filters.Add<OperationCancelledExceptionFilter>();
                    }
                );
                //.AddJsonOptions(
                //    options =>
                //    {
                //        options.JsonSerializerOptions.WriteIndented = true;
                //        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy?.CamelCase;
                //    });

                //.AddNewtonsoftJson(options =>
                //{
                //    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //    options.SerializerSettings.ContractResolver = new CamelCaseNullToStringPropertyNamesContractResolver();

                //    var converter = new StringEnumConverter(namingStrategy: new CamelCaseNamingStrategy());
                //    options.SerializerSettings.Converters.Add(converter);
                //});
        }
    }
}
