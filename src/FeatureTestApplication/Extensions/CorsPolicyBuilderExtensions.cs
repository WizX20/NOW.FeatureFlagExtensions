using Microsoft.AspNetCore.Cors.Infrastructure;

namespace FeatureTestApplication.Extensions
{
    public static class CorsPolicyBuilderExtensions
    {
        /// <summary>
        /// Allow all CORS origins.
        /// </summary>
        /// <param name="builder">The current <see cref="CorsPolicyBuilder"/>.</param>
        public static CorsPolicyBuilder SetAllowAllOrigins(this CorsPolicyBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return SetAllowedOrigins(builder, "*");
        }

        /// <summary>
        /// Sets the allowed-origins properties, based on the <paramref name="allowedOrigins"/>.
        /// </summary>
        /// <param name="builder">The current <see cref="CorsPolicyBuilder"/>.</param>
        /// <param name="allowedOrigins">Asterisk (*) as to allow all, or, a comma-separated list of allowed origins.</param>
        public static CorsPolicyBuilder SetAllowedOrigins(this CorsPolicyBuilder builder, string allowedOrigins)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrWhiteSpace(allowedOrigins))
            {
                throw new ArgumentException($"'{nameof(allowedOrigins)}' cannot be null or whitespace.", nameof(allowedOrigins));
            }

            // Translate the wildcard/asterisk to an allow-all function.
            if (allowedOrigins == "*")
            {
                //Logger.Warn($"CORS: The active configuration (appsettings.json) uses an allow-all rule for the CORS policy; " +
                //    $"this is a potential risk, please consider specifying a more detailed set of allowed origins.");
                builder.SetIsOriginAllowed(_ => true);
                return builder;
            }

            string[] allowedOriginsValues = allowedOrigins.Split(new char[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries);
            builder.WithOrigins(allowedOriginsValues).SetIsOriginAllowedToAllowWildcardSubdomains();

            return builder;
        }
    }
}