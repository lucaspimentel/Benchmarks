using RegexNonBacktracking;

if (args.Length > 0 && args[0] == "test")
{
    var benchmarks = new Benchmarks();
    benchmarks.Setup();

    var defaultRegexResult = benchmarks.CompileDefaultRegex();
    var experimentalRegexResult = benchmarks.CompileExperimentalRegex();

    System.Console.WriteLine(defaultRegexResult == experimentalRegexResult);
}
else
{
    _ = BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();
// _ = BenchmarkDotNet.Running.BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}
