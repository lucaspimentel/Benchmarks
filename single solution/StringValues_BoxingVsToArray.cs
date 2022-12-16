using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Primitives;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
//[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class StringValues_BoxingVsToArray
{
    private StringValues values = new("foo");

    [Benchmark]
    public void Box()
    {
        Foo(values);
    }

    [Benchmark]
    public void ToArray()
    {
        Foo(values.ToArray());
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void Foo(IEnumerable<string> values)
    {
    }
}
