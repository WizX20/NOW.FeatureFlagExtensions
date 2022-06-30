using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FeatureTestBenchmarks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#if DEBUG
BenchmarkRunner.Run<DefaultInjectionBenchmarks>(
    config: ManualConfig.CreateMinimumViable().WithOptions(ConfigOptions.DisableOptimizationsValidator)
);
#else
BenchmarkRunner.Run<DefaultInjectionBenchmarks>();
#endif
