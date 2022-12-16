using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class GetByteCount
{
    private const int Limit = 200;
    private static readonly Encoding Encoding = new UTF8Encoding(false);
    private static readonly char[] Value = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lorem mollis aliquam ut porttitor leo a. Imperdiet nulla malesuada pellentesque elit.".ToCharArray();

    [Benchmark]
    public bool Progressive()
    {
        for (var i = 0; i < Value.Length; i++)
        {
            if (Encoding.GetByteCount(Value, 0, i) > Limit)
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool EachChar()
    {
        int length = 0;

        for (var i = 0; i < Value.Length; i++)
        {
            length += Encoding.GetByteCount(Value, i, 1);

            if (length > Limit)
            {
                return true;
            }
        }

        return false;
    }
}
