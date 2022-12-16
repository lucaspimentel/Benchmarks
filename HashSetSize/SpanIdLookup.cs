using System;
using System.Collections.Generic;

namespace Benchmarks;

public enum SearchType
{
    Default,
    ArraySegmentOfSpan,
    HashSetOfIds
}

public readonly struct SpanIdLookup
{
    // for small trace chunks, use the ArraySegment<Span> copy directly, no heap allocations
    private readonly ArraySegment<Span> _spans = default;

    // for large trace chunks, fall back to HashSet<ulong>, more heap allocations
    private readonly HashSet<ulong> _hashSet = default;

    public SpanIdLookup(ArraySegment<Span> spans, SearchType type = SearchType.Default)
    {
        if (spans.Count == 0)
        {
            // Contains() will always return false
            return;
        }

        _spans = spans;

        // netfx       50
        // net 6    40-50
        // net 3.1     50+

        if (type == SearchType.HashSetOfIds || (type == SearchType.Default && spans.Count > 50))
        {
            // for large trace chunks, fall back to HashSet<ulong>
/*
#if NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            _hashSet = new HashSet<ulong>(spans.Count);
#else // NETFX < 4.7.2 || NETSTANDARD < 2.1
            _hashSet = new HashSet<ulong>();
#endif
*/

#if NETFRAMEWORK
            _hashSet = new HashSet<ulong>();
#else
            _hashSet = new HashSet<ulong>(spans.Count);
#endif

            for (var i = 0; i < spans.Count; i++)
            {
                _hashSet.Add(spans.Array![i + spans.Offset].SpanId);
            }
        }
    }

    public bool Contains(ulong value)
    {
        if (_spans.Count == 0)
        {
            return false;
        }

        if (_hashSet != null)
        {
            return _hashSet?.Contains(value) ?? false;
        }

        // special case for trace chunks with single spans.
        // the local root span also the first span and the most common parent in most traces.
        if (value == _spans.Array![_spans.Offset].SpanId)
        {
            return true;
        }

        // if we didn't create a HashSet, iterate over the span array.
        // Using a for loop to avoid the boxing allocation on ArraySegment.GetEnumerator
        for (var i = 0; i < _spans.Count; i++)
        {
            if (value == _spans.Array![i + _spans.Offset].SpanId)
            {
                return true;
            }
        }

        return false;
    }
}
