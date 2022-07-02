#if !DEBUG
using BenchmarkDotNet.Running;
#endif

using NOW.FeatureFlagExtensions.Benchmarks.VanillaInjection;

#if DEBUG
var benchmarks = new Benchmarks();
benchmarks.Setup();
benchmarks.GetTestServiceSingleton();
benchmarks.GetTestServiceScoped();
benchmarks.GetTestServiceTransient();
#else
BenchmarkRunner.Run<Benchmarks>();
#endif
