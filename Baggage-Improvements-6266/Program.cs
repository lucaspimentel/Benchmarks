using System;
using BaggageImprovements6266;

_ = BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();
// _ = BenchmarkDotNet.Running.BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

// var benchmarks = new Benchmarks { ItemCount = 10 };
// benchmarks.Setup();
//
// while(true)
// {
//     benchmarks.After();
// }
