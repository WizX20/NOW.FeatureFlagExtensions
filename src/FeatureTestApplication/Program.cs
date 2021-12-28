using Ardalis.ListStartupServices;
using FeatureTestApplication;
using FeatureTestApplication.Configuration;
using FeatureTestApplication.Configurations;
using FeatureTestApplication.Controllers;
using FeatureTestApplication.Extensions.ServiceCollection;
using FeatureTestApplication.Extensions.WebHostEnvironment;
using FeatureTestApplication.Services;
using FeatureTestApplication.Swagger.OperationFilters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.ApiVersioning.Extensions;
using NOW.FeatureFlagExtensions.ApiVersioning.Middleware;
using NOW.FeatureFlagExtensions.ApiVersioning.Swagger.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using NOW.FeatureFlagExtensions.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.FeatureManagement.Swagger.OperationFilters;
using NOW.FeatureTestApplication.Extensions.ServiceCollection;

/*
    Start of application start-up.
*/
var builder = WebApplication.CreateBuilder(args);

// Load application settings.
var environment = builder.Environment;
var configuration = environment.GetConfigurationRoot();
var appSettings = builder.Services.RegisterAppSettingsConfiguration(configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDefaultMvcOptions();
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
    var defaultApiVersion = configuration.GetDefaultApiVersion(nameof(AppSettingsConfiguration.ApiVersioning), Constants.ApiVersioning.DefaultApiVersion);
    builder.Services.AddSingleton(apiVersioningOptions);

    builder.Services.AddApiVersioning(options =>
    {
        options.SetDefaultApiVersioningOptions(defaultApiVersion);
        options.Conventions.Controller<TestController>().HasApiVersion(defaultApiVersion);
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureApiVersioningSwaggerGenOptions();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();

    if (apiVersioningOptions != null)
    {
        options.SetDefaultApiVersioningOptions();
    }

    options.OperationFilter<FeatureFilterHeaderParameter>(); // Add a header operation filter which sets feature switches.
    options.OperationFilter<FileResultContentTypeOperationFilter>();
    options.OperationFilter<ValidateRequiredParameters>();
});

// Register feature management and filters using Microsoft.FeatureManagement.
builder.Services.AddFeatureManagement()
    .AddFeatureFlagExtensionsFeatureFilters();

builder.Services.AddFeatureFlagFeatureManagementManager();

builder.Services.AddScoped<ITestConfiguration, TestConfigurationOne>(
    new ImplementationFeature<ITestConfiguration>(typeof(TestConfigurationTwo), FeatureFlags.ReleaseFlags.TestFeature.TestConfigurationTwo),
    new ImplementationFeature<ITestConfiguration>(typeof(TestConfigurationThree), FeatureFlags.ReleaseFlags.TestFeature.TestConfigurationThree)
);

builder.Services.AddTransient<ITestService, TestServiceOne>(
    new ImplementationFeature<ITestService>(typeof(TestServiceTwo), FeatureFlags.ReleaseFlags.TestFeature.TestServiceTwo),
    new ImplementationFeature<ITestService>(typeof(TestServiceThree), FeatureFlags.ReleaseFlags.TestFeature.TestServiceThree)
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
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseVersionedSwagger();
        app.UseVersionedSwaggerUI(apiVersionDescriptionProvider);
    }
    else
    {
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