using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Extensions
{
    public static class WebHostEnvironmentExtensions
    {
        public static Task<bool> EvaluateEnvironmentAsync(
            this IWebHostEnvironment environment,
            string[] environments)
        {
            if (environment == null || environments.Length < 1)
            {
                return Task.FromResult(false);
            }

            // Enable feature if any of the environments matches with the active one.
            var isEnabled = environments.Any(env => environment.IsEnvironment(env));

            return Task.FromResult(isEnabled);
        }
    }
}