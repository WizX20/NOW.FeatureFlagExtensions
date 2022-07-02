#if !DEBUG
using BenchmarkDotNet.Running;
#endif

using NOW.FeatureFlagExtensions.Benchmarks.InterceptorInjection;

#if DEBUG
var benchmarks = new Benchmarks();
benchmarks.Setup();
benchmarks.GetTestServiceSingleton();
benchmarks.GetTestServiceScoped();
benchmarks.GetTestServiceTransient();
#else
BenchmarkRunner.Run<Benchmarks>();
#endif
