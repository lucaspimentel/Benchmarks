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
    public class StringContains2
    {
        [Params("http://foo.azurewebsites.net/admin", "http://foo.azurewebsites.net/Admin")]
        public string Url { get; set; }

        string[] PatternsMatch = { "foo.bar", "example.com", "test", "azurewebsites.net" };

        string[] PatternsNoMatch = { "foo.bar", "example.com", "test", "azurewebsites1.net" };

        [Benchmark]
        public bool IndexOf_Match()
        {
            for (var y = 0; y < PatternsMatch.Length; y++)
            {
                if (Url.IndexOf(PatternsMatch[y], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        [Benchmark]
        public bool Replace_Match()
        {
            var urlLower = Url.ToLowerInvariant();

            for (var y = 0; y < PatternsMatch.Length; y++)
            {
                var found = (urlLower.Length - urlLower.Replace(PatternsMatch[y], string.Empty).Length) / PatternsMatch[y].Length > 0;

                if (found)
                {
                    return true;
                }
            }

            return false;
        }

        [Benchmark]
        public bool IndexOf_NoMatch()
        {
            for (var y = 0; y < PatternsNoMatch.Length; y++)
            {
                if (Url.IndexOf(PatternsNoMatch[y], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        [Benchmark]
        public bool Replace_NoMatch()
        {
            var urlLower = Url.ToLowerInvariant();

            for (var y = 0; y < PatternsNoMatch.Length; y++)
            {
                var found = (urlLower.Length - urlLower.Replace(PatternsNoMatch[y], string.Empty).Length) / PatternsNoMatch[y].Length > 0;

                if (found)
                {
                    return true;
                }
            }

            return false;
        }
    }
}