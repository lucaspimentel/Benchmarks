using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ListDictionary;

[SimpleJob(RuntimeMoniker.Net461)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    [Params(5, 10, 30)]
    public int ItemCount { get; set; }

    private List<KeyValuePair<string, string>> _listBaggage;
    private Dictionary<string, string> _dictionaryBaggage;
    private string[] _keys;
    private string _existingKey;

    [GlobalSetup]
    public void Setup()
    {
        _listBaggage = new List<KeyValuePair<string, string>>(ItemCount);
        _dictionaryBaggage = new Dictionary<string, string>(ItemCount);
        _keys = new string[ItemCount];

        for (int i = 0; i < ItemCount; i++)
        {
            var key = $"key{i}";
            var value = $"value{i}";
            _keys[i] = key;
            _listBaggage.Add(new KeyValuePair<string, string>(key, value));
            _dictionaryBaggage[key] = value;
        }

        // Select an existing key to test lookup performance
        _existingKey = _keys[ItemCount / 2];
    }

    [Benchmark]
    public void AddOrReplace_List()
    {
        var key = "newKey";
        var value = "newValue";
        bool found = false;

        for (int i = 0; i < _listBaggage.Count; i++)
        {
            if (_listBaggage[i].Key == key)
            {
                _listBaggage[i] = new KeyValuePair<string, string>(key, value);
                found = true;
                break;
            }
        }

        if (!found)
        {
            _listBaggage.Add(new KeyValuePair<string, string>(key, value));
        }
    }

    [Benchmark]
    public void AddOrReplace_Dictionary()
    {
        var key = "newKey";
        var value = "newValue";
        _dictionaryBaggage[key] = value;
    }

    [Benchmark]
    public string Iterate_List()
    {
        string temp = null;

        foreach (var item in _listBaggage)
        {
            temp = item.Key;
        }

        return temp;
    }

    [Benchmark]
    public string Iterate_Dictionary()
    {
        string temp = null;

        foreach (var kvp in _dictionaryBaggage)
        {
            temp = kvp.Key;
        }

        return temp;
    }

    [Benchmark]
    public string Lookup_List()
    {
        foreach (var item in _listBaggage)
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
        _dictionaryBaggage.TryGetValue(_existingKey, out var temp);
        return temp;
    }

    [Benchmark]
    public void Remove_List()
    {
        for (int i = 0; i < _listBaggage.Count; i++)
        {
            if (_listBaggage[i].Key == _existingKey)
            {
                _listBaggage.RemoveAt(i);
                break;
            }
        }
    }

    [Benchmark]
    public void Remove_Dictionary()
    {
        _dictionaryBaggage.Remove(_existingKey);
    }
}
