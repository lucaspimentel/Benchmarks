using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net47)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    public class StringBuilderAppend
    {
        private readonly KeyValuePair<string, string>[] pairs =
        {
            new("key1", "value1"),
            new("key2", "value2"),
            new("key3", "value3"),
            new("key4", "value4"),
        };

        private readonly StringBuilder _builder = new(100);

        [Benchmark]
        public void Append()
        {
            _builder.Clear();

            foreach (var pair in pairs)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(',');
                }

                _builder.Append(pair.Key);
                _builder.Append('=');
                _builder.Append(pair.Value);
            }
        }

        [Benchmark]
        public void AppendInterpolatedStrings()
        {
            _builder.Clear();

            foreach (var pair in pairs)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(',');
                }

                _builder.Append($"{pair.Key}={pair.Value}");
            }
        }

        [Benchmark]
        public void AppendFormat()
        {
            _builder.Clear();

            foreach (var pair in pairs)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(',');
                }

                _builder.AppendFormat("{0}={1}", pair.Key, pair.Value);
            }
        }
    }
}
