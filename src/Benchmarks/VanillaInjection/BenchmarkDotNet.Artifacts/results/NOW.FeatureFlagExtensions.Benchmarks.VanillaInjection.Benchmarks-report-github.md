``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|                  Method |     Mean |   Error |  StdDev | Rank |  Gen 0 | Allocated |
|------------------------ |---------:|--------:|--------:|-----:|-------:|----------:|
|    GetTestServiceScoped | 494.7 ns | 7.72 ns | 6.84 ns |    1 | 0.1001 |     840 B |
| GetTestServiceSingleton | 505.8 ns | 5.34 ns | 4.46 ns |    2 | 0.1001 |     840 B |
| GetTestServiceTransient | 507.0 ns | 1.96 ns | 1.84 ns |    2 | 0.1030 |     864 B |
