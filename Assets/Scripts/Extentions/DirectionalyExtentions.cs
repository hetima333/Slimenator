using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static void AddOrReplace<TKey, TValue>
    (
        this Dictionary<TKey, TValue> self,
        TKey key,
        TValue value
    )
    {
        self[key] = value;
    }
}