using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class ParseEnum
{
    [Params("1", "a", "Sunday")]
    public string Strings;

    [Benchmark]
    public void IntTryParse()
    {
        _ = int.TryParse(Strings, out int value);
    }

    [Benchmark]
    public void EnumTryParse()
    {
        _ = Enum.TryParse(Strings, out DayOfWeek value);
    }
}
