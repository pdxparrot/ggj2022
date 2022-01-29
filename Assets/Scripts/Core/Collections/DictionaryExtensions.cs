using System.Collections.Generic;

using JetBrains.Annotations;

namespace pdxpartyparrot.Core.Collections
{
    public static class DictionaryExtensions
    {
        public static TV GetOrAdd<TK, TV>(this Dictionary<TK, TV> dict, TK key) where TV : new()
        {
            if(dict.TryGetValue(key, out var value)) {
                return value;
            }

            value = new TV();
            dict.Add(key, value);
            return value;
        }
    }
}
