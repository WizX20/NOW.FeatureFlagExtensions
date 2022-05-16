![Snapshooter](https://raw.githubusercontent.com/WizX20/NOW.FeatureFlagExtensions/master/res/banner/banner.png?token=GHSAT0AAAAAABUKDU6UD5FLWBDLMULB3WWCYTZSBAQ)

**NOW FeatureFlag Extensions is a set of extensions to support feature switching**

## Getting started

Clone the repository and run the included *FeatureTestApplication*, the default start-page will be the swagger documentation page,where you 
can play around with the registered test-features.

Additionally, you can visit `/allservices`, to display all service registrations.


## Features

### Api Versioning

#### NOW.FeatureFlagExtensions.ApiVersioning

Simplifies setup and configuration for API Version behavior with `Microsoft.AspNetCore.Mvc.Versioning`. The middleware provided combines
path (_URL_), query-string and header version-readers.
- When the API Version is provided via the query-string, this version will have preference over the version in the path;
- When the API Version is provided via the header, this version will have preference over the version in the path;
- When the API Version is provided via both the query-string and the header, and they do not contain an identical version, a
  **"400 - AmbiguousApiVersion"** error will be returned;
- When both the query-string and the header are missing the API version, the on from the path, or otherwise, default, API Version will
  be used.

This package contains configuration options, to set a default Api Version and Api Description properties. Additionally, middleware is 
provided, to append the active Api Version to the Api response headers.

The following snippet gives an example of the configuration options available.

```json
{
  ...
  "ApiVersioning": {
    "DefaultVersion": {
      "Major": 1,
      "Minor": 0
    },
    "ApiDescription": {
      "Title": "Feature Test API",
      "Description": "",
      "Contact": {
        "Name": "Company Name",
        "Email": "info@company.com"
      },
      "License": {
        "Name": "MIT",
        "Url": "https://mit-license.org/"
      }
    }
  },
  ...
```

In the startup of your application, wire up and enable the API versioning extensions.

```csharp
// Add Api versioning.
var apiVersioningOptions = appSettings.ApiVersioning;
if (apiVersioningOptions != null)
{
    builder.Services.AddSingleton(apiVersioningOptions);

    var defaultApiVersion = configuration.GetDefaultApiVersion(
        configurationSection: nameof(AppSettingsConfiguration.ApiVersioning),
        defaultApiVersionFallback: Constants.ApiVersioning.DefaultApiVersion
    );

    builder.Services.AddApiVersioning(options =>
    {
        options.SetDefaultApiVersioningOptions(defaultApiVersion);
        
        // Versioned controller examples.
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
```

In the example:
- Use the `appSettings` to fetch the `ApiVersioning` section.
  - The example uses [confuration binding](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.bind?view=dotnet-plat-ext-6.0).
    , you can review the `FeatureTestApplication.Extensions.ServiceCollection.ConfigurationExtensions` for an example.
- Register the `appSettings` as singleton, so we can use the configured values throughout the application.
  - _Currently the middleware (`ApiVersionMiddleware`) and Swagger extensions (`NOW.FeatureFlagExtensions.ApiVersioning.Swagger` package )
    depend on this registration. If you are not planning on using any of these features, then you can skip this._
- Get the `defaultApiVersion` using the `ConfigurationExtensions` from the package.
- Use the `Microsoft.AspNetCore.Mvc.Versioning` extensions to register the options:
  - Use the `ApiVersioningOptionsExtensions` from the package to set the defaults.
  - Some examples using the `Microsoft.AspNetCore.Mvc.Versioning` `IDeclareApiVersionConventionBuilder`, to register a API version
    for test-controllers. A better known method to do this is by using the [ApiControllerAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute?view=aspnetcore-6.0)
- Use the `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer` extensions to set the APi format.<br>
  _Currently the middleware (`ApiVersionMiddleware`) depends on this exact format. If you do not plan on using this feature, then you can 
  skip this part._
  - Configure the string that is used to format an API version (as a group name);
  - Enable API version substitution, to enable our wanted format.

Optionally, register the `ApiVersionMiddleware`, which will report back the API version that was used during the request, by adding it to
the response headers.

```csharp
/*
    Start of the HTTP request pipeline configuration.
*/
var app = builder.Build();

...

// Configure middleware.
app.UseMiddleware<ApiVersionMiddleware>();
```

#### NOW.FeatureFlagExtensions.ApiVersioning.Swagger

This package contains extensions to add a 'Versioned Swagger' and 'Versioned Swagger UI' documentation, using the 
`NOW.FeatureFlagExtensions.ApiVersioning` package.

TODO IMAGE

```csharp
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureApiVersioningSwaggerGenOptions();
builder.Services.AddSwaggerGen(options =>
{
    ...

    if (apiVersioningOptions != null)
    {
        options.SetDefaultApiVersioningOptions();
    }

    ...
});
```

EXPLAIN

```csharp
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

    app.UseDeveloperExceptionPage();
}

// Configure middleware.
app.UseMiddleware<ApiVersionMiddleware>();
```

EXPLAIN

### Dependency Injection

#### NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement

`NOW.FeatureFlagExtensions.DependencyInjection` to build own impl.

#### NOW.FeatureFlagExtensions.DependencyInjection.Interceptors

A


### Microsoft FeatureManagement

#### NOW.FeatureFlagExtensions.FeatureManagement

A

#### NOW.FeatureFlagExtensions.FeatureManagement.Swagger

A
