using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Datadog.Trace.Util;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
//[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class Iterating
{
    private readonly string[] _array = { "one" };

    [Benchmark]
    public string Enumerable()
    {
        IEnumerable<string> values = _array;
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string Array()
    {
        string[] values = _array;
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string Iterator()
    {
        IEnumerable<string> GetIterator()
        {
            foreach (var value in _array)
            {
                yield return value;
            }
        }
        IEnumerable<string> values = GetIterator();
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }


    [Benchmark]
    public string StringEnumerable_Enumerable()
    {
        var values = new StringEnumerable((IEnumerable<string>)_array);
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string StringEnumerable_Array()
    {
        var values = new StringEnumerable(_array);
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string StringEnumerable_String()
    {
        var values = new StringEnumerable("one");
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string StringEnumerable_Empty()
    {
        var values = StringEnumerable.Empty;
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }

    [Benchmark]
    public string StringEnumerable_Null()
    {
        var values = new StringEnumerable((string)null);
        string result = null;

        foreach (var value in values)
        {
            result = value;
        }

        return result;
    }
}
