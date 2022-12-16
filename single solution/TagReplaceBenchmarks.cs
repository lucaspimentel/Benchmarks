using System;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [MemoryDiagnoser]
    public class TagReplaceBenchmarks
    {
        const string TagName = "This.is#My%Tag";

        //[Benchmark]
        public void Regex_ToLower()
        {
            _ = Regex.Replace(input: TagName, "[^a-zA-Z0-9_:/-]", replacement: "_").ToLowerInvariant();
        }

        [Benchmark]
        public void ToLower_Regex()
        {
            _ = Regex.Replace(input: TagName.ToLowerInvariant(), "[^a-z0-9_:/-]", replacement: "_");
        }

        [Benchmark]
        public void ToLower_CompiledRegex()
        {
            _ = Regex.Replace(input: TagName.ToLowerInvariant(), "[^a-z0-9_:/-]", replacement: "_", RegexOptions.Compiled);
        }

        [Benchmark]
        public void ToLower_For_StringBuilder()
        {
            var sb = new StringBuilder(TagName.ToLowerInvariant());

            for (int x = 0; x < sb.Length; x++)
            {
                switch (sb[x])
                {
                    case >= 'a' and <= 'z' or >= '0' and <= '9' or '_' or ':' or '/' or '-':
                        continue;
                    default:
                        sb[x] = '_';
                        break;
                }
            }

            _ = sb.ToString();
        }
    }
}
