using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class ListNoThreadBenchmark
    {
        private static List<int> list;
        private static int[] array;
        static readonly int size = 1000 * 1000;

        static ListNoThreadBenchmark()
        {
            list = new List<int>(size);
            array = new int[size];

            for (int i = 0; i < size; i++)
            {
                list.Add(42);
                array[i] = 42;
            }
        }

        [Benchmark]
        public void UseList()
        {
            var start = 0;
            var end = list.Count;

            for (var j = start; j < end; j++)
            {
                list[j] = 42;
            }
        }

        [Benchmark]
        public void UseArray()
        {
            var start = 0;
            var end = array.Length;

            for (var j = start; j < end; j++)
            {
                array[j] = 42;
            }
        }
    }
}