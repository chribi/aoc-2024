using System.Numerics;

namespace LibAoc;

public static class DictionaryExtensions {
    public static void AddOrSet<K, V>(this Dictionary<K, V> dict, K key, V value)
        where K: notnull
        where V : INumber<V>
    {
        if (dict.TryGetValue(key, out var v)) {
            dict[key] = v + value;
        } else {
            dict[key] = value;
        }
    }
}

