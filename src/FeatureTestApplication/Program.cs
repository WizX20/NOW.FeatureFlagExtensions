using Ardalis.ListStartupServices;
using FeatureTestApplication;
using FeatureTestApplication.Configuration;
using FeatureTestApplication.Controllers;
using FeatureTestApplication.Extensions.ServiceCollection;
using FeatureTestApplication.Extensions.WebHostEnvironment;
using FeatureTestApplication.Swagger.OperationFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using NOW.ApiVersioning.Extensions;
using NOW.ApiVersioning.Middleware;
using NOW.ApiVersioning.Swagger.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.FeatureManagement.Swagger.OperationFilters;
using NOW.FeatureTestApplication.Extensions.ServiceCollection;
using DefaultInjectionFlags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.DefaultInjection;
using DefaultInjectionTests = TestFeatures.DefaultInjection;
using InterceptorInjectionFlags = TestFeatures.FeatureFlags.ReleaseFlags.TestFeatures.InterceptorInjection;
using InterceptorInjectionTests = TestFeatures.InterceptorInjection;

/*
    Start of application start-up.
*/
var builder = WebApplication.CreateBuilder(args);

// Load application settings.
var environment = builder.Environment;
var configuration = environment.GetConfigurationRoot();
var appSettings = builder.Services.RegisterAppSettingsConfiguration(configuration);

// Add services to the container.
builder.Services.AddDefaultControllerOptions();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddEndpointsApiExplorer();

// Add middleware that renders all injected services.
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);
    config.Path = "/allservices";
});

// Add response caching.
var responseCachingOptions = appSettings.FeatureTestApplication?.ResponseCaching;
if (responseCachingOptions != null)
{
    builder.Services.AddResponseCaching(options =>
    {
        options.MaximumBodySize = responseCachingOptions.MaximumBodySize;
        options.UseCaseSensitivePaths = responseCachingOptions.UseCaseSensitivePaths;
    });
}

// Add Api versioning.
var apiVersioningOptions = appSettings.ApiVersioning;
if (apiVersioningOptions != null)
{
    builder.Services.TryAddSingleton(apiVersioningOptions);

    var defaultApiVersion = configuration.GetDefaultApiVersion(
        configurationSection: nameof(AppSettingsConfiguration.ApiVersioning),
        defaultApiVersionFallback: Constants.ApiVersioning.DefaultApiVersion
    );

    builder.Services.AddApiVersioning(options =>
    {
        options.SetDefaultApiVersioningOptions(defaultApiVersion);
        options.Conventions.Controller<TestController>().HasApiVersion(defaultApiVersion);
        options.Conventions.Controller<FeatureTestApplication.Controllers.v1_1.TestController>().HasApiVersion(new ApiVersion(1, 1));
    });

    builder.Services.AddVersionedApiExplorer(options =>
    {
        // Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]".
        options.GroupNameFormat = "'v'VVV";

        // Note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates.
        options.SubstituteApiVersionInUrl = true;
    });
}

// Configure versioned SwaggerGen options.
if (apiVersioningOptions != null)
{
    builder.Services.ConfigureApiVersioningSwaggerGenOptions();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();

    // Configures swagger for support with versioning via the path, query or header.
    if (apiVersioningOptions != null)
    {
        options.SetDefaultApiVersioningOptions();
    }

    // Add a header operation filter which sets feature switches.
    options.OperationFilter<FeatureFilterHeaderParameter>();

    // Example filters for file-result and validation.
    options.OperationFilter<FileResultContentTypeOperationFilter>();
    options.OperationFilter<ValidateRequiredParameters>();
});

// Register feature management and filters using Microsoft.FeatureManagement.
builder.Services.AddFeatureManagement()
    .AddFeatureFlagExtensionsFeatureFilters();

builder.Services.AddFeatureFlagFeatureManagementManager();

builder.Services.AddScoped<DefaultInjectionTests.ScopedFeature.ITestServiceScoped, DefaultInjectionTests.ScopedFeature.TestServiceOneScoped>(
    new FeatureFlagWrapper<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>(
        typeof(DefaultInjectionTests.ScopedFeature.TestServiceTwoScoped),
        DefaultInjectionFlags.TestDefaultServiceScopedTwo),
    new FeatureFlagWrapper<DefaultInjectionTests.ScopedFeature.ITestServiceScoped>(
        typeof(DefaultInjectionTests.ScopedFeature.TestServiceThreeScoped),
        DefaultInjectionFlags.TestDefaultServiceScopedThree)
);

builder.Services.AddTransient<DefaultInjectionTests.TransientFeature.ITestServiceTransient, DefaultInjectionTests.TransientFeature.TestServiceOneTransient>(
    new FeatureFlagWrapper<DefaultInjectionTests.TransientFeature.ITestServiceTransient>(
        typeof(DefaultInjectionTests.TransientFeature.TestServiceTwoTransient),
        DefaultInjectionFlags.TestDefaultServiceTransientTwo),
    new FeatureFlagWrapper<DefaultInjectionTests.TransientFeature.ITestServiceTransient>(
        typeof(DefaultInjectionTests.TransientFeature.TestServiceThreeTransient),
        DefaultInjectionFlags.TestDefaultServiceTransientThree)
);

builder.Services.AddInterceptedScoped<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped, InterceptorInjectionTests.ScopedFeature.TestServiceOneScoped>(
    new FeatureFlagWrapper<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>(
        typeof(InterceptorInjectionTests.ScopedFeature.TestServiceTwoScoped),
        InterceptorInjectionFlags.TestInterceptedServiceScopedTwo),
    new FeatureFlagWrapper<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped>(
        typeof(InterceptorInjectionTests.ScopedFeature.TestServiceThreeScoped),
        InterceptorInjectionFlags.TestInterceptedServiceScopedThree)
);

builder.Services.AddInterceptedSingleton<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton, InterceptorInjectionTests.SingletonFeature.TestServiceOneSingleton>(
    new FeatureFlagWrapper<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>(
        typeof(InterceptorInjectionTests.SingletonFeature.TestServiceTwoSingleton),
        InterceptorInjectionFlags.TestInterceptedServiceSingletonTwo),
    new FeatureFlagWrapper<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton>(
        typeof(InterceptorInjectionTests.SingletonFeature.TestServiceThreeSingleton),
        InterceptorInjectionFlags.TestInterceptedServiceSingletonThree)
);

builder.Services.AddInterceptedTransient<InterceptorInjectionTests.TransientFeature.ITestServiceTransient, InterceptorInjectionTests.TransientFeature.TestServiceOneTransient>(
    new FeatureFlagWrapper<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>(
        typeof(InterceptorInjectionTests.TransientFeature.TestServiceTwoTransient),
        InterceptorInjectionFlags.TestInterceptedServiceTransientTwo),
    new FeatureFlagWrapper<InterceptorInjectionTests.TransientFeature.ITestServiceTransient>(
        typeof(InterceptorInjectionTests.TransientFeature.TestServiceThreeTransient),
        InterceptorInjectionFlags.TestInterceptedServiceTransientThree)
);

// Register basic application services.
builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddDefaultCorsPolicy(appSettings);

/*
    Start of the HTTP request pipeline configuration.
*/
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (apiVersioningOptions != null)
    {
        // Add versioned Swagger docs using the FeatureFlagExtensions package.
        app.UseVersionedSwagger();
        app.UseVersionedSwaggerUI();
    }
    else
    {
        // Add default Swagger docs.
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseShowAllServicesMiddleware();
    app.UseDeveloperExceptionPage();
}

// Configure middleware.
app.UseMiddleware<ApiVersionMiddleware>();

// Configure routing.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();