using System;
using System.Text;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace StringEquality
{
    [SimpleJob(RuntimeMoniker.Net461)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class Benchmarks
    {
        private readonly Random Random = new Random();
        private readonly string String1;
        private readonly string String2;
        private readonly string String3;
        private readonly string String4;
        private readonly string String5;

        public Benchmarks()
        {
            var filler = GetRandomString(1000);
            String1 = "a" + filler;
            String2 = "b" + filler;
            String3 = filler + "a";
            String4 = filler + "b";
            String5 = filler + "b";
        }

        public string GetRandomString(int size)
        {
            var sb = new StringBuilder(size);

            for (int x = 0; x < size; x++)
            {
                sb.Append((char)Random.Next(32, 127));
            }

            return sb.ToString();
        }

        [Benchmark]
        public void Compare_1_2()
        {
            // Implement your benchmark here
            var b = String1 == String2;
        }

        [Benchmark]
        public void Compare_3_4()
        {
            // Implement your benchmark here
            var b = String3 == String4;
        }

        [Benchmark]
        public void Compare_4_5()
        {
            // Implement your benchmark here
            var b = String4 == String5;
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                var summary = BenchmarkRunner.Run<Benchmarks>();
            }
        }
    }
}
