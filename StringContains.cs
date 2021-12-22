using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [MemoryDiagnoser]
    public class StringContains
    {
        public const string Value = "Lorem ipsum dolor sit amet, consectetur match adipiscing elit.";

        [Params("match", "no_match", "MATCH", "NO_MATCH")]
        public string Substring { get; set; }

        [Benchmark]
        public bool IndexOf()
        {
            return Value.IndexOf(Substring, StringComparison.OrdinalIgnoreCase) > 0;
        }

        [Benchmark]
        public bool Replace()
        {
            return (Value.Length - Value.Replace(Substring, string.Empty).Length) / Substring.Length > 0;
        }
    }
}