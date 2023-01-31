using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace TryGetSingle;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    [Params(0, 1, 2)]
    public int Count;

    [Params(true, false)]
    public bool UseList;

    public IEnumerable<string> Values;

    [GlobalSetup]
    public void Setup()
    {
        var list = new List<string>(Count);

        for (int i = 0; i < Count; i++)
        {
            list.Add("test");
        }

        Values = UseList ? list : list.Select(x => x);
    }

    [Benchmark]
    public string TryGetSingle()
    {
        var values = Values;

        var list = values as IReadOnlyList<string> ?? values.Take(2).ToList();

        if (list.Count == 1)
        {
            return list[0] ?? string.Empty;
        }

        return string.Empty;
    }

    [Benchmark]
    public string TryGetSingle2()
    {
        var values = Values;

        if (values is IReadOnlyList<string> list)
        {
            if (list.Count == 1)
            {
                return list[0] ?? string.Empty;
            }

            return string.Empty;
        }

        _ = TryGetSingleRare(values, out var value);
        return value;
    }

    private bool TryGetSingleRare(IEnumerable<string> values, out string value)
    {
        value = string.Empty;
        var hasValue = false;

        foreach (var s in values)
        {
            if (!hasValue)
            {
                // save first item
                value = s ?? string.Empty;
                hasValue = true;
            }
            else
            {
                // we already saved the first item and there is a second one
                return false;
            }
        }

        // there were no item
        return false;
    }
}
