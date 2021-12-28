using Ardalis.ListStartupServices;
using FeatureTestApplication;
using FeatureTestApplication.Configurations;
using FeatureTestApplication.Extensions.ServiceCollection;
using FeatureTestApplication.Extensions.WebHostEnvironment;
using FeatureTestApplication.Services;
using FeatureTestApplication.Swagger.OperationFilters;
using Microsoft.FeatureManagement;
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
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);
    config.Path = "/allservices";
});

// Configure output caching and serializer behavior.

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();

    //options.DocumentFilter<RemoveDefaultApiVersionRouteDocumentFilter>(); // Remove duplicate routes, due to /v{version:} routes on controllers.
    //options.OperationFilter<DefaultVersionValuesParameter>(); // Add a custom operation filter which sets default values.
    //options.OperationFilter<RemoveQueryApiVersionParamOperationFilter>(); // Remove required 'version' path field.

    //options.OperationFilter<AcceptLanguageHeaderParameter>(); // Add a header operation filter which sets the active culture.
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
//builder.Services.RegisterDefaultLocalization(appSettings);

/*
    Start of application start-up configuration.
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseShowAllServicesMiddleware();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();