
![FeatureFlag Extensions Banner](res/banner/banner.png)

**NOW FeatureFlag Extensions is a set of extensions to support feature switching**

What if you want to alter, or re-write a (part) of a service, factory, or any other class, that
is registered in your application via (Microsoft) [Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). But, you want to use feature switches/toggles, without having to alter existing code in the class, and without having to restart the running application in order to test the experimental/new code? This question produced this (experimental) project and it's additional libraries.

<br>

# Getting started

Clone the repository and run the included *FeatureTestApplication*, the default start-page will be the swagger documentation page, where you can play around with the registered test-features in combination with Api Versioning.

Additionally, you can visit `/allservices`, to display all service registrations. This endpoint
uses the package [Ardalis.ListStartupServices](https://github.com/ardalis/AspNetCoreStartupServices).


# Features

## Dependency Injection

A set of packages to enable "hot-reload" feature switching via Dependency Injection and some other, additional, extensions.

- [NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement](#todo)<br>
  Enables "hot-reload" feature switching via Dependency Injection. This specific version depends on [Microsoft FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet). [read more...](#todo)
  - Depends on: `NOW.FeatureFlagExtensions.DependencyInjection`<br>
    Uses standard Dependency Injection features to enable switching, currently supports `AddScoped` and `AddTransient` extensions (no `Singleton` at this point). [read more...](#todo)
    - Depends on:
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Managers`](#todo)<br>
        Contains the abstract class `FeatureFlagManager`, if you want to enable switching via a feature switching/toggle library of your choice. Inherit this class and make it work! [read more...](#todo)
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Models`](#todo)<br>
        Contains the models used by the core libraries. [read more...](#todo)

- [NOW.FeatureFlagExtensions.DependencyInjection.Interceptors](#todo)<br>
  An alternative/addition for the `NOW.FeatureFlagExtensions.DependencyInjection` extensions. You can use this one, the other one, or use them both depending on your use case. This package will add the extensions `AddInterceptedScoped`, `AddInterceptedSingleton` and `AddInterceptedTransient`. The extensions depend on the [Castle.Core](https://github.com/castleproject/Core) interceptors to do the heavy lifting. *This will probably add some extra overhead, but no benchmarks have been done yet...* [read more...](#todo)
    - Depends on:
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Managers`](#todo)<br>
        Contains the abstract class `FeatureFlagManager`, if you want to enable switching via a feature switching/toggle library of your choice. Inherit this class and make it work! [read more...](#todo)
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Models`](#todo)<br>
        Contains the models used by the core libraries. [read more...](#todo)

## Microsoft FeatureManagement

A set of extensions and additional filters, that build on the out-of-the-box [Microsoft FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) configuration.

  - [NOW.FeatureFlagExtensions.FeatureManagement](#todo)<br>
    Add additional configurable filters like the `AggregateFeatureFilter`, `ClaimsFeatureFilter`, `EnvironementsFeatureFilter` and `RequestHeadersFeatureFilter`. [read more...](#todo)

  - [NOW.FeatureFlagExtensions.FeatureManagement.Swagger](#todo)<br>
    Adds a `FeatureFilterHeaderParameter` for swagger, to enable easy feature switching via dropdown(s), that is (are) populated with all available feature switches. [read more...](#todo)

## Api Versioning

- [NOW.FeatureFlagExtensions.ApiVersioning](src/ApiVersioning/readme.md)<br>
  Simplifies setup and configuration for API Version behavior with `Microsoft.AspNetCore.Mvc.Versioning`. The middleware provided combines path (_URL_), query-string and header version-readers. [read more...](src/ApiVersioning/readme.md)

- [NOW.FeatureFlagExtensions.ApiVersioning.Swagger](src/ApiVersioning.Swagger/readme.md)<br>
  This package contains extensions to add a 'Versioned Swagger' and 'Versioned Swagger UI' documentation, using the `NOW.FeatureFlagExtensions.ApiVersioning` package. [read more...](src/ApiVersioning.Swagger/readme.md)


# Community

This project has adopted the code of conduct defined by the [Contributor Covenant](https://contributor-covenant.org/) to clarify expected behavior in our community. For more information, see the [FeatureFlag Extensions Code of Conduct](docs/CODE_OF_CONDUCT.md).
