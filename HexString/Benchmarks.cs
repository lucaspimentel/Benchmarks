using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Datadog.Trace.DataStreamsMonitoring.Utils;
using Datadog.Trace.Util;

namespace HexString;

[SimpleJob(RuntimeMoniker.Net60)]
// [SimpleJob(RuntimeMoniker.NetCoreApp31)]
// [SimpleJob(RuntimeMoniker.Net461)]
[MemoryDiagnoser]
public class Benchmarks
{
    private const string String = "1234567890abcdef";

    [Benchmark]
    public ulong UInt64_TryParse() // *** faster in .net core 3.1+ ***
    {
        bool success = ulong.TryParse(String, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result);

        if (success)
        {
            return result;
        }

        ThrowHelper.ThrowException("UInt64.TryParse() failed");
        return 0; // unreachable code
    }

#if NET6_0_OR_GREATER
    [Benchmark]
    public ulong Convert_FromHexString() // *** slowest and allocates ***
    {
        byte[] bytes = Convert.FromHexString(String);
        ulong value = BitConverter.ToUInt64(bytes);
        return BitConverter.IsLittleEndian ? BinaryPrimitivesHelper.ReverseEndianness(value) : value;
    }
#endif

    [Benchmark]
    public ulong HexString_TryParseUInt64() // *** faster in .net fx ***
    {
        bool success = Datadog.Trace.Util.HexString.TryParseUInt64(String, out var result);

        if (success)
        {
            return result;
        }

        ThrowHelper.ThrowException("HexString.TryParseUInt64() failed");
        return 0; // unreachable code
    }

    private const ulong Value = ulong.MaxValue / 2;

    [Benchmark]
    public string UInt64_ToString() // *** slowest ***
    {
        return Value.ToString("x16");
    }

#if NET6_0_OR_GREATER
    [Benchmark]
    public string Convert_ToHexString() // *** 2nd fastest (how??), with allocations ***
    {
        ulong value = BitConverter.IsLittleEndian ? BinaryPrimitivesHelper.ReverseEndianness(Value) : Value;
        Span<ulong> ulongs = stackalloc ulong[] { value };
        Span<byte> bytes = System.Runtime.InteropServices.MemoryMarshal.AsBytes(ulongs);
        return Convert.ToHexString(bytes);
    }
#endif

    [Benchmark]
    public string HexString_ToHexString() // *** 3rd fastest, with allocations ***
    {
        return Datadog.Trace.Util.HexString.ToHexString(Value, lowerCase: true);
    }

#if NETCOREAPP3_1_OR_GREATER
    [Benchmark]
    public int HexString_ToHexChars() // *** fastest, no allocations ***
    {
        Span<ulong> ulongs = stackalloc ulong[] { Value };
        Span<byte> bytes = System.Runtime.InteropServices.MemoryMarshal.AsBytes(ulongs);

        Span<char> chars = stackalloc char[16];
        Datadog.Trace.Util.HexString.ToHexChars(bytes, chars, lowerCase: true);

        return chars.Length;
    }
#endif
}
