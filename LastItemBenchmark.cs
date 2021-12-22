using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [MemoryDiagnoser]
    public class LastItemBenchmark
    {
        const string Value = "This.is.a.test";

        [Benchmark]
        public string Last()
        {
            return Value.Split('.').Last();
        }

        [Benchmark]
        public string UseArray()
        {
            var array = Value.Split('.');
            return array[array.Length - 1];
        }

        [Benchmark]
        public string Substring()
        {
            return Value.Substring(Value.LastIndexOf('.'));
        }
    }
}