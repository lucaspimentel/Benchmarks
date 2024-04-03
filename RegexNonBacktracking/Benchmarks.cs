using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RegexNonBacktracking;

[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
[MemoryDiagnoser]
// [IterationsColumn]
public class Benchmarks
{
    private const string QueryStringShort = "password=123&" +
                                            "pwd=123&" +
                                            "pwdx=123&" +
                                            "passphrase=123&" +
                                            "passphrasex=123&" +
                                            "signature=123&" +
                                            "signaturex=123&" +
                                            "authentication=123&" +
                                            "authenticationx=123&" +
                                            "authorization=123&" +
                                            "authorizationx=123&" +
                                            "query1=query%7Bhero%7Bname+appearsIn%7D%7D&";

    private const string QueryStringLong =
        "password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&password=123&pwd=123&pwdx=123&passphrase=123&passphrasex=123&signature=123&signaturex=123&authentication=123&authenticationx=123&authorization=123&authorizationx=123&query1=query%7Bhero%7Bname+appearsIn%7D%7D&";

    private const string Replacement = "<redacted>";

    // private Regex _defaultRegex;

    // private Regex _experimentalRegex;

    [GlobalSetup]
    public void Setup()
    {
        // System.AppDomain.CurrentDomain.SetData("REGEX_NONBACKTRACKING_MAX_AUTOMATA_SIZE", 2000);

        // _defaultRegex = new Regex(QueryStringObfuscatorRegex.DefaultRegexPattern,
        //     QueryStringObfuscatorRegex.DefaultOptions);
        //
        // _experimentalRegex = new Regex(QueryStringObfuscatorRegex.ExperimentalRegexPattern,
        //     QueryStringObfuscatorRegex.DefaultOptions);
    }

    // [Benchmark]
    // public string ReplaceDefaultRegex()
    // {
    //     return _defaultRegex.Replace(QueryStringShort, Replacement);
    // }

    // [Benchmark]
    // public string ReplaceDefaultRegexLong()
    // {
    //     return _defaultRegex.Replace(QueryStringLong, Replacement);
    // }

    // [Benchmark]
    // public string ReplaceExperimentalRegex()
    // {
    //     return _experimentalRegex.Replace(QueryStringShort, Replacement);
    // }

    // [Benchmark]
    // public string ReplaceExperimentalRegexLong()
    // {
    //     return _experimentalRegex.Replace(QueryStringLong, Replacement);
    // }

    [Benchmark(Baseline = true)]
    public string CompileDefaultRegex()
    {
        var regex = new Regex(QueryStringObfuscatorRegex.DefaultRegexPattern,
            QueryStringObfuscatorRegex.DefaultOptions);
        return regex.Replace("", Replacement);
    }

    [Benchmark]
    public string CompileExperimentalRegex()
    {
        var regex = new Regex(QueryStringObfuscatorRegex.ExperimentalRegexPattern,
            QueryStringObfuscatorRegex.DefaultOptions);
        return regex.Replace("", Replacement);
    }
}
