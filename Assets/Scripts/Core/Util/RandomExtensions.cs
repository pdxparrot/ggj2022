using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace pdxpartyparrot.Core.Util
{
    public static class RandomExtensions
    {
        #region Random Collection Entries

        [CanBeNull]
        public static T GetRandomEntry<T>(this Random random, IReadOnlyCollection<T> collection)
        {
            if(collection.Count < 1) {
                return default;
            }

            int idx = random.Next(collection.Count);
            return collection.ElementAt(idx);
        }

        [CanBeNull]
        public static T RemoveRandomEntry<T>(this Random random, IList<T> collection)
        {
            if(collection.Count < 1) {
                return default;
            }

            int idx = random.Next(collection.Count);
            T v = collection.ElementAt(idx);
            collection.RemoveAt(idx);
            return v;
        }

        #endregion

        // 0 or 1
        public static int CoinFlip(this Random random)
        {
            return random.NextBool() ? 1 : 0;
        }

        // 1 or -1
        public static float NextSign(this Random random)
        {
            return random.NextBool() ? 1 : -1;
        }

        // true or false
        public static bool NextBool(this Random random)
        {
            return random.Next(2) != 0;
        }

        // [0.0, 1.0)
        public static float NextSingle(this Random random)
        {
            return (float)random.NextDouble();
        }

        // [0.0, maxValue)
        public static float NextSingle(this Random random, float maxValue)
        {
            return random.NextSingle(0.0f, maxValue);
        }

        // [minValue, maxValue)
        public static float NextSingle(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble(minValue, maxValue);
        }

        // [0.0, maxValue)
        public static double NextDouble(this Random random, double maxValue)
        {
            return random.NextDouble() * maxValue;
        }

        // [minValue, maxValue)
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return minValue + random.NextDouble() * (maxValue - minValue);
        }
    }
}
