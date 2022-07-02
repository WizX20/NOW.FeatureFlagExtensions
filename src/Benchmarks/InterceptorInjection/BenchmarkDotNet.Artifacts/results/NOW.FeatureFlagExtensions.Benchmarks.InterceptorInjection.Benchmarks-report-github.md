``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|                  Method |        Mean |     Error |    StdDev | Rank |  Gen 0 | Allocated |
|------------------------ |------------:|----------:|----------:|-----:|-------:|----------:|
| GetTestServiceSingleton |    26.92 ns |  0.094 ns |  0.083 ns |    1 |      - |         - |
|    GetTestServiceScoped |    29.03 ns |  0.351 ns |  0.328 ns |    2 |      - |         - |
| GetTestServiceTransient | 1,245.65 ns | 13.712 ns | 12.826 ns |    3 | 0.1335 |   1,120 B |
