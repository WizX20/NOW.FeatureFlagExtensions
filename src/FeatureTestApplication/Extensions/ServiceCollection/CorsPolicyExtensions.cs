using FeatureTestApplication;
using FeatureTestApplication.Configuration;
using FeatureTestApplication.Extensions;

namespace NOW.FeatureTestApplication.Extensions.ServiceCollection
{
    public static class CorsPolicyExtensions
    {
        /// <summary>
        /// Adds a <see cref="Constants.CorsPolicies.DefaultCorsPolicy"/> CORS policy, based on the <paramref name="appSettings"/>.
        /// </summary>
        public static void AddDefaultCorsPolicy(this IServiceCollection services, AppSettingsConfiguration appSettings)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            AddCorsPolicy(services, Constants.CorsPolicies.DefaultCorsPolicy, appSettings);
        }

        /// <summary>
        /// Adds a CORS policy, based on the <paramref name="appSettings"/>.
        /// </summary>
        public static void AddCorsPolicy(this IServiceCollection services, string policyName, AppSettingsConfiguration appSettings)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(policyName))
            {
                throw new ArgumentException($"'{nameof(policyName)}' cannot be null or empty.", nameof(policyName));
            }

            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            var allowedOrigins = appSettings.FeatureTestApplication?.CorsPolicy?.AllowedOrigins;
            if (string.IsNullOrEmpty(allowedOrigins))
            {
                throw new ArgumentException($"Missing '{nameof(allowedOrigins)}' configuration; cannot be null or empty.", nameof(allowedOrigins));
            }

            // Configure CORS policy, for more information see:
            // - https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
            // - https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-3.1#cross-origin-connections
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                    builder
                        .AllowCredentials()
                        .SetAllowedOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }
    }
}