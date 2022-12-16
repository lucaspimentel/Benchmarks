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
    public class ListBenchmark
    {
        private static List<int> list;
        private static int[] array;
        static readonly int size = 1000 * 1000;

        [Params(1)]
        public int ThreadCount { get; set; }

        static ListBenchmark()
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
            var watch = Stopwatch.StartNew();
            var threads = new List<Thread>();

            for (var i = 0; i < ThreadCount; i++)
            {
                var t = new Thread(idx =>
                {
                    var index = (int)idx;
                    var start = index * (size / ThreadCount);
                    var end = start + (size / ThreadCount);

                    for (var j = start; j < end; j++)
                    {
                        list[j] = 42;
                    }
                });

                t.Start(i);
                threads.Add(t);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        [Benchmark]
        public void UseArray()
        {
            var watch = Stopwatch.StartNew();
            var threads = new List<Thread>(ThreadCount);

            for (var i = 0; i < ThreadCount; i++)
            {
                var t = new Thread(idx =>
                {
                    var index = (int)idx;
                    var start = index * (size / ThreadCount);
                    var end = start + (size / ThreadCount);

                    for (var j = start; j < end; j++)
                    {
                        array[j] = 42;
                    }
                });

                t.Start(i);
                threads.Add(t);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}