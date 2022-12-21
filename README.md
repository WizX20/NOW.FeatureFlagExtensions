
![FeatureFlag Extensions Banner](res/banner/banner.png)

**NOW FeatureFlag Extensions is a set of extensions to support feature switching**

What if you want to alter, or re-write a (part) of a service, factory, or any other class, that
is registered in your application via (Microsoft) [Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). But, you want to use feature switches/toggles, without having to alter existing code in the class, and without having to restart the running application in order to test the experimental/new code? This question produced this (experimental) project and it's additional libraries.

Build Status
------------

Branch | Status
--- | :---:
master | [![master](https://github.com/WizX20/NOW.FeatureFlagExtensions/actions/workflows/ci.yml/badge.svg?branch=master&event=push)](https://github.com/WizX20/NOW.FeatureFlagExtensions/actions/workflows/ci.yml)

NuGet Packages
---------------------------

| Package Name | .NET 7 |
| ------------ | :-----------: |
| **Dependency Injection** |
| *Main* |
| [FeatureManagement][NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.nuget] | 1.2.0 Preview |
| [Interceptors][NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.nuget] | 1.2.0 Preview |
| *Core* |
| [DependencyInjection][NOW.FeatureFlagExtensions.DependencyInjection.nuget] | 1.2.0 Preview |
| [Managers][NOW.FeatureFlagExtensions.DependencyInjection.Managers.nuget] | 1.2.0 Preview |
| [Models][NOW.FeatureFlagExtensions.DependencyInjection.Models.nuget] | 1.2.0 Preview |
| **Microsoft FeatureManagement** |
| [FeatureManagement][NOW.FeatureFlagExtensions.FeatureManagement.nuget] | 1.2.0 Preview |
| [FeatureManagement.Swagger][NOW.FeatureFlagExtensions.FeatureManagement.Swagger.nuget] | 1.2.0 Preview |


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
  An alternative/addition for the `NOW.FeatureFlagExtensions.DependencyInjection` extensions. You can use this one, the other one, or use them both depending on your use case. This package will add the extensions `AddInterceptedScoped`, `AddInterceptedSingleton` and `AddInterceptedTransient`. The extensions depend on the [Castle.Core](https://github.com/castleproject/Core) interceptors to do the heavy lifting. [read more...](#todo)
    - Depends on:
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Managers`](#todo)<br>
        Contains the abstract class `FeatureFlagManager`, if you want to enable switching via a feature switching/toggle library of your choice. Inherit this class and make it work! [read more...](#todo)
      - [`NOW.FeatureFlagExtensions.DependencyInjection.Models`](#todo)<br>
        Contains the models used by the core libraries. [read more...](#todo)

### Benchmarks

*Vanilla Feature Flag Usage*
https://github.com/WizX20/NOW.FeatureFlagExtensions/blob/2ac0c7d188aa6f93ff0b93c7d3092e6debcf2dd8/src/Benchmarks/VanillaInjection/BenchmarkDotNet.Artifacts/results/NOW.FeatureFlagExtensions.Benchmarks.VanillaInjection.Benchmarks-report-github.md?plain=1#L11-L15

*FeatureFlagExtensions (Default) Usage*
https://github.com/WizX20/NOW.FeatureFlagExtensions/blob/2ac0c7d188aa6f93ff0b93c7d3092e6debcf2dd8/src/Benchmarks/DefaultInjection/BenchmarkDotNet.Artifacts/results/NOW.FeatureFlagExtensions.Benchmarks.DefaultInjection.Benchmarks-report-github.md?plain=1#L11-L14

*FeatureFlagExtensions Interceptors Usage*
https://github.com/WizX20/NOW.FeatureFlagExtensions/blob/2ac0c7d188aa6f93ff0b93c7d3092e6debcf2dd8/src/Benchmarks/InterceptorInjection/BenchmarkDotNet.Artifacts/results/NOW.FeatureFlagExtensions.Benchmarks.InterceptorInjection.Benchmarks-report-github.md?plain=1#L11-L15


## Microsoft FeatureManagement

A set of extensions and additional filters, that build on the out-of-the-box [Microsoft FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) configuration.

  - [NOW.FeatureFlagExtensions.FeatureManagement](#todo)<br>
    Add additional configurable filters like the `AggregateFeatureFilter`, `ClaimsFeatureFilter`, `EnvironementsFeatureFilter` and `RequestHeadersFeatureFilter`. [read more...](#todo)

  - [NOW.FeatureFlagExtensions.FeatureManagement.Swagger](#todo)<br>
    Adds a `FeatureFilterHeaderParameter` for swagger, to enable easy feature switching via dropdown(s), that is (are) populated with all available feature switches. [read more...](#todo)


# Community

This project has adopted the code of conduct defined by the [Contributor Covenant](https://contributor-covenant.org/) to clarify expected behavior in our community. For more information, see the [Code of Conduct](docs/CODE_OF_CONDUCT.md).


[NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement
[NOW.FeatureFlagExtensions.DependencyInjection.Interceptors.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.DependencyInjection.Interceptors
[NOW.FeatureFlagExtensions.DependencyInjection.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.DependencyInjection
[NOW.FeatureFlagExtensions.DependencyInjection.Managers.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.DependencyInjection.Managers
[NOW.FeatureFlagExtensions.DependencyInjection.Models.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.DependencyInjection.Models

[NOW.FeatureFlagExtensions.FeatureManagement.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.FeatureManagement
[NOW.FeatureFlagExtensions.FeatureManagement.Swagger.nuget]: https://www.nuget.org/packages/NOW.FeatureFlagExtensions.FeatureManagement.Swagger