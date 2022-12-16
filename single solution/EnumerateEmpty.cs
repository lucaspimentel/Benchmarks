using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class EnumerateEmpty
{
    [Benchmark]
    public void ArrayEmpty()
    {
        IEnumerable<int> values = Array.Empty<int>();

        foreach (int value in values)
        {
            Foo(value);
        }
    }

    [Benchmark]
    public void EnumerableEmpty()
    {
        IEnumerable<int> values = Enumerable.Empty<int>();

        foreach (int value in values)
        {
            Foo(value);
        }
    }

    private void Foo<T>(T value)
    {
    }
}
