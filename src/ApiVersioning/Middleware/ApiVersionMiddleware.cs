using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOW.FeatureFlagExtensions.ApiVersioning.Configuration;

namespace NOW.FeatureFlagExtensions.ApiVersioning.Middleware
{
    public class ApiVersionMiddleware
    {
        private readonly RequestDelegate _next;
        private ApiVersioningOptions _apiVersioningOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiVersionMiddleware(
            RequestDelegate next,
            ApiVersioningOptions apiVersioningOptions,
            IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _apiVersioningOptions = apiVersioningOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return;
            }

            ApiVersion? apiVersion = null;

            if (_httpContextAccessor.HttpContext.Response?.Headers != null &&
                !_httpContextAccessor.HttpContext.Response.Headers.ContainsKey(Constants.HttpHeaders.ApiVersion))
            {
                apiVersion = _httpContextAccessor.HttpContext.GetRequestedApiVersion();
            }

            if (apiVersion == null && _apiVersioningOptions.DefaultVersion != null)
            {
                apiVersion = new ApiVersion(
                    _apiVersioningOptions.DefaultVersion.Major,
                    _apiVersioningOptions.DefaultVersion.Minor
                );
            }

            if (_httpContextAccessor.HttpContext.Response != null && apiVersion != null)
            {
                _httpContextAccessor.HttpContext.Response.Headers.Add(
                    Constants.HttpHeaders.ApiVersion.ToLowerInvariant(),
                    apiVersion.ToString("'v'VVV"));
            }

            await _next(context);
        }
    }
}