using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ToBoolean
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }

    [SimpleJob(RuntimeMoniker.Net48)]
    public class Benchmarks
    {
        [Params("", "T", "A", "TRUE", "BAR")]
        public string Value { get; set; }

        [Benchmark(Baseline = true)]
        public void ToBoolean_OLD()
        {
            ToBoolean_OLD(Value);
        }

        [Benchmark]
        public void ToBoolean_NEW()
        {
            ToBoolean_NEW(Value);
        }

        public static bool? ToBoolean_NEW(string value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            if (value.Length == 0)
            {
                return null;
            }

            if (value.Length == 1)
            {
                if (value[0] == 'T' || value[0] == 't' ||
                    value[0] == 'Y' || value[0] == 'y' ||
                    value[0] == '1')
                {
                    return true;
                }

                if (value[0] == 'F' || value[0] == 'f' ||
                    value[0] == 'N' || value[0] == 'n' ||
                    value[0] == '0')
                {
                    return false;
                }

                return null;
            }

            if (string.Equals(value, "TRUE", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "YES", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(value, "FALSE", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "NO", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return null;
        }

        public static bool? ToBoolean_OLD(string value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            if (value.Length == 1)
            {
                if (value[0] == 'T' || value[0] == 't' ||
                    value[0] == 'Y' || value[0] == 'y' ||
                    value[0] == '1')
                {
                    return true;
                }

                if (value[0] == 'F' || value[0] == 'f' ||
                    value[0] == 'N' || value[0] == 'n' ||
                    value[0] == '0')
                {
                    return false;
                }

                return null;
            }

            if (string.Equals(value, "TRUE", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "YES", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(value, "FALSE", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "NO", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return null;
        }
    }
}
