using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace FooBar;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    [Params(0, 1, 2)]
    public int Foo;

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public string Bar1()
    {
        return "";
    }

    [Benchmark]
    public string Bar2()
    {
        return "";
    }
}
