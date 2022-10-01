``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.978/21H2)
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|                  Method |        Mean |     Error |    StdDev | Rank |   Gen0 | Allocated |
|------------------------ |------------:|----------:|----------:|-----:|-------:|----------:|
| GetTestServiceSingleton |    24.94 ns |  0.129 ns |  0.121 ns |    1 |      - |         - |
|    GetTestServiceScoped |    27.80 ns |  0.220 ns |  0.195 ns |    2 |      - |         - |
| GetTestServiceTransient | 1,790.26 ns | 24.408 ns | 19.056 ns |    3 | 0.2613 |    2192 B |
