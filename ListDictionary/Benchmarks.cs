using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Datadog.Trace;

namespace ListDictionary;

//[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    [Params(1, 4, 8, 16, 32)]
    public int ItemCount { get; set; }

    [Benchmark]
    public void List()
    {
        var baggage1 = new Baggage();

        // add items upstream
        for (int i = 0; i < ItemCount; i++)
        {
            baggage1[$"k{i}"] = $"v{i}";
        }

        // iterate to send downstream
        foreach (var pair in baggage1)
        {
            _ = pair.Value;
            _ = pair.Key;
        }

        var baggage2 = new Baggage();

        // add items downstream
        foreach (var pair in baggage1)
        {
            baggage2[pair.Key] = pair.Value;
        }

        // lookup each item
        for (int i = 0; i < ItemCount; i++)
        {
            _ = baggage2[$"k{i}"];
        }
    }

    [Benchmark]
    public void Dictionary()
    {
        var baggage1 = new Dictionary<string, string>();

        // add items upstream
        for (int i = 0; i < ItemCount; i++)
        {
            baggage1[$"k{i}"] = $"v{i}";
        }

        // iterate to send downstream
        foreach (var pair in baggage1)
        {
            _ = pair.Value;
            _ = pair.Key;
        }

        var baggage2 = new Dictionary<string, string>();

        // add items downstream
        foreach (var pair in baggage1)
        {
            baggage2[pair.Key] = pair.Value;
        }

        // lookup each item
        for (int i = 0; i < ItemCount; i++)
        {
            _ = baggage2[$"k{i}"];
        }
    }

    /*
    private List<KeyValuePair<string, string>> _list;
    private Dictionary<string, string> _dictionary;
    private string _existingKey;

    [GlobalSetup]
    public void Setup()
    {
        _list = new List<KeyValuePair<string, string>>(ItemCount);
        _dictionary = new Dictionary<string, string>(ItemCount);
        var keys = new string[ItemCount];

        for (int i = 0; i < ItemCount; i++)
        {
            var key = $"key{i}";
            var value = $"value{i}";
            keys[i] = key;
            _list.Add(new KeyValuePair<string, string>(key, value));
            _dictionary[key] = value;
        }

        // Select an existing key to test lookup performance
        _existingKey = keys[ItemCount * 2 / 3];
    }

    [Benchmark]
    public List<KeyValuePair<string, string>> Add_List()
    {
        var list = new List<KeyValuePair<string, string>>();

        for (int i = 0; i < ItemCount; i++)
        {
            list.Add(new KeyValuePair<string, string>($"key{i}", $"value{i}"));
        }

        return list;
    }

    [Benchmark]
    public Dictionary<string, string> Add_Dictionary()
    {
        var dictionary = new Dictionary<string, string>();

        for (int i = 0; i < ItemCount; i++)
        {
            dictionary[$"key{i}"] = $"value{i}";
        }

        return dictionary;
    }

    [Benchmark]
    public void AddOrReplace_List()
    {
        const string key = "newKey";
        const string value = "newValue";
        bool found = false;

        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Key == key)
            {
                _list[i] = new KeyValuePair<string, string>(key, value);
                found = true;
                break;
            }
        }

        if (!found)
        {
            _list.Add(new KeyValuePair<string, string>(key, value));
        }
    }

    [Benchmark]
    public void AddOrReplace_Dictionary()
    {
        _dictionary["newKey"] = "newValue";
    }

    [Benchmark]
    public string Iterate_List()
    {
        string temp = null;

        foreach (var item in _list)
        {
            temp = item.Key;
        }

        return temp;
    }

    [Benchmark]
    public string Iterate_Dictionary()
    {
        string temp = null;

        foreach (var kvp in _dictionary)
        {
            temp = kvp.Key;
        }

        return temp;
    }

    [Benchmark]
    public string Lookup_List()
    {
        foreach (var item in _list)
        {
            if (item.Key == _existingKey)
            {
                return item.Value;
            }
        }

        return null;
    }

    [Benchmark]
    public string Lookup_Dictionary()
    {
        _dictionary.TryGetValue(_existingKey, out var temp);
        return temp;
    }

    [Benchmark]
    public void Remove_List()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Key == _existingKey)
            {
                _list.RemoveAt(i);
                break;
            }
        }
    }

    [Benchmark]
    public void Remove_Dictionary()
    {
        _dictionary.Remove(_existingKey);
    }
    */
}
