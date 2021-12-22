using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net461)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class ThrowHelper
    {
        [Params(true, false)]
        public bool IsNull
        {
            get => RefType == null;
            set
            {
                if (value)
                {
                    RefType = null;
                    NullableValueType = null;
                }
                else
                {
                    RefType = new object();
                    NullableValueType = 1;
                }
            }
        }

        public object RefType;

        public int ValueType = 1;

        public int? NullableValueType;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowArgumentNullExceptionIfNull_Object(object argument, [CallerArgumentExpression("argument")] string paramName = null)
        {
            if (argument is null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowArgumentNullExceptionIfNull_Generic<T>(T argument, [CallerArgumentExpression("argument")] string paramName = null)
        {
            if (argument == null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        //[Benchmark]
        public void ThrowNew()
        {
            try
            {
                if (IsNull)
                {
                    throw new ArgumentNullException("paramName");
                }
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullException()
        {
            try
            {
                if (IsNull)
                {
                    ThrowArgumentNullException("paramName");
                }
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Object_ReferenceType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Object(RefType);
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Object_ValueType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Object(ValueType);
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Object_NullableValueType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Object(NullableValueType);
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Generic_ReferenceType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Generic(RefType);
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Generic_ValueType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Generic(ValueType);
            }
            catch
            {
            }
        }

        [Benchmark]
        public void ThrowArgumentNullExceptionIfNull_Generic_NullableValueType()
        {
            try
            {
                ThrowArgumentNullExceptionIfNull_Generic(NullableValueType);
            }
            catch
            {
            }
        }
    }
}

#if !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}
#endif
