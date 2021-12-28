using Microsoft.AspNetCore.Mvc;

namespace FeatureTestApplication
{
    public static class Constants
    {
        public static class ApiVersioning
        {
            /// <summary>
            /// Fallback for the default Api version.
            /// </summary>
            public static ApiVersion DefaultApiVersion = new(1, 0);
        }

        public static class CorsPolicies
        {
            /// <summary>
            /// Key for the default CORS policy.
            /// </summary>
            public const string DefaultCorsPolicy = nameof(DefaultCorsPolicy);
        }

        public static class ResponseCache
        {
            /// <summary>
            /// Key for default cache policy.
            /// </summary>
            public const string DefaultCacheProfile = nameof(DefaultCacheProfile);
        }
    }
}