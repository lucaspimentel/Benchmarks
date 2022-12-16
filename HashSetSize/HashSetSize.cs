using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser(displayGenColumns: false)]
public class HashSetSize
{
    // primes: 3, 7, 11, 17, 23, 29, 37, 47, 59
    [Params(40, 50, 60)]
    public int Size { get; set; }

    [Params(SearchType.ArraySegmentOfSpan, SearchType.HashSetOfIds)]
    public SearchType Type { get; set; }

    public ArraySegment<Span> Spans;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var rng = new Random();
        var bytes = new byte[8];
        var spans = new Span[Size];

        for (var i = 0; i < spans.Length; i++)
        {
            rng.NextBytes(bytes);
            var id = BitConverter.ToUInt64(bytes, startIndex: 0);
            spans[i] = new Span(id);
        }

        Spans = new ArraySegment<Span>(spans);
    }

    [Benchmark]
    public bool SpanIdLookup()
    {
        var collection = new SpanIdLookup(Spans, Type);
        bool discard = false;

        for (var i = 0; i < Spans.Count; i++)
        {
            Span span = Spans.Array![i + Spans.Offset];
            discard = collection.Contains(span.SpanId);
        }

        return discard;
    }
}
