using Microsoft.AspNetCore.Http;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static Task<bool> EvaluateClaimsAsync(
            this IHttpContextAccessor httpContext,
            string[] requiredClaims)
        {
            if (requiredClaims == null || requiredClaims.Length < 1)
            {
                return Task.FromResult(false);
            }

            // Retrieve the current user (ClaimsPrincipal).
            var user = httpContext.HttpContext?.User;
            if (user == null || user.Claims == null || !user.Claims.Any())
            {
                return Task.FromResult(false);
            }

            // Only enable the feature if the user has ALL the required claims.
            var isEnabled = requiredClaims
                .All(claimType => user.HasClaim(claim => claim.Type == claimType));

            return Task.FromResult(isEnabled);
        }

        public static Task<bool> EvaluateHeadersAsync(
            this IHttpContextAccessor httpContext,
            string[] requiredHeaders)
        {
            if (requiredHeaders == null || requiredHeaders.Length < 1)
            {
                return Task.FromResult(false);
            }

            // Retrieve the current request headers.
            var requestHeaders = httpContext.HttpContext?.Request?.Headers;
            if (requestHeaders == null || requestHeaders.Count < 1)
            {
                return Task.FromResult(false);
            }

            // Only enable the feature if the user has ALL the required headers,
            // and those headers have a value that indicates that they are enabled.
            var isEnabled = requiredHeaders
                .All(headerKey =>
                    requestHeaders.ContainsKey(headerKey) &&
                    requestHeaders.TryGetValue(headerKey, out var headerValue) &&
                    bool.TryParse(headerValue, out var enabledResult) &&
                    enabledResult == true
                );

            return Task.FromResult(isEnabled);
        }
    }
}