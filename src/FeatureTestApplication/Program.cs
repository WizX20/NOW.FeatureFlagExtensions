using NOW.FeatureFlagExtensions.DependencyInjection.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.Extensions;
using NOW.FeatureFlagExtensions.DependencyInjection.Models;
using FeatureTestApplication;
using FeatureTestApplication.Configurations;
using FeatureTestApplication.Services;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register feature management and filters using Microsoft.FeatureManagement.
builder.Services.AddFeatureManagement();
//  .AddFeatureFilter<ClaimsFeatureFilter>()
//  .AddFeatureFilter<EnvironementsFeatureFilter>()
//  .AddFeatureFilter<RequestHeadersFeatureFilter>()
//  .AddFeatureFilter<CombinedFeatureFilter>();

builder.Services.AddFeatureFlagFeatureManagementManager();

builder.Services.AddSingleton<ITestConfiguration, TestConfigurationOne>(
    new ImplementationFeature<ITestConfiguration>(typeof(TestConfigurationTwo), FeatureFlags.ReleaseFlags.TestFeature.TestConfigurationTwo),
    new ImplementationFeature<ITestConfiguration>(typeof(TestConfigurationThree), FeatureFlags.ReleaseFlags.TestFeature.TestConfigurationThree)
);

builder.Services.AddTransient<ITestService, TestServiceOne>(
    new ImplementationFeature<ITestService>(typeof(TestServiceTwo), FeatureFlags.ReleaseFlags.TestFeature.TestServiceTwo),
    new ImplementationFeature<ITestService>(typeof(TestServiceThree), FeatureFlags.ReleaseFlags.TestFeature.TestServiceThree)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();