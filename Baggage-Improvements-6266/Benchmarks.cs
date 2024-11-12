using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Datadog.Trace;
using Datadog.Trace.Propagators;

namespace BaggageImprovements6266;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    [Params(1, 5)]
    public int ItemCount { get; set; }

    [Params(false, true)]
    public bool WithEncoding { get; set; }

    private Baggage Baggage { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Baggage = new Baggage();

        for (int i = 0; i < ItemCount; i++)
        {
            if (WithEncoding)
            {
                Baggage[$"k{i}"] = $"value é🐶{i}";
            }
            else
            {
                Baggage[$"k{i}"] = $"value{i}";
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void Before()
    {
        W3CBaggagePropagator1.CreateHeader(Baggage);
    }

    [Benchmark]
    public void After3()
    {
        W3CBaggagePropagator3.CreateHeader(Baggage);
    }

    [Benchmark]
    public void After4()
    {
        W3CBaggagePropagator4.CreateHeader(Baggage);
    }

    [Benchmark]
    public void After5()
    {
        W3CBaggagePropagator5.CreateHeader(Baggage);
    }
}
