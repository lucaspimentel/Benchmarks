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

    private Baggage Baggage { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Baggage = new Baggage();

        for (int i = 0; i < ItemCount; i++)
        {
            Baggage[$"k{i}"] = $"v{i}";
            // Baggage[$"k{i}"] = $"v é🐶{i}";
        }
    }

    [Benchmark(Baseline = true)]
    public void Before()
    {
        W3CBaggagePropagator1.CreateHeader(Baggage);
    }

    [Benchmark]
    public void After()
    {
        W3CBaggagePropagator3.CreateHeader(Baggage);
    }
}
