using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
//[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class YieldVsNewArray
{
    [Benchmark]
    public void Yield()
    {
        _ = YieldImpl("foo");
    }

    private IEnumerable<string> YieldImpl(string value)
    {
        yield return value;
    }

    [Benchmark]
    public void NewArray()
    {
        _ = NewArrayImpl("foo");
    }

    private IEnumerable<string> NewArrayImpl(string value)
    {
        return new[] { value };
    }
}
