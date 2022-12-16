using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class GetTypeFullName
{
    private static string[] FullName = { typeof(GetTypeFullName).FullName };

	[Benchmark]
    public string Reflection()
    {
        return typeof(GetTypeFullName).FullName;
    }

	[Benchmark]
    public string Cached()
    {
        return FullName[0];
    }
}
