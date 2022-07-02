#if !DEBUG
using BenchmarkDotNet.Running;
#endif

using NOW.FeatureFlagExtensions.Benchmarks.DefaultInjection;

#if DEBUG
var benchmarks = new Benchmarks();
benchmarks.Setup();
benchmarks.GetTestServiceScoped();
benchmarks.GetTestServiceTransient();
#else
BenchmarkRunner.Run<Benchmarks>();
#endif
