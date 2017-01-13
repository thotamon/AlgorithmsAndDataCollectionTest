using System;
using System.Collections.Generic;
using System.Linq;

namespace Sum
{
    public struct Pair<T1, T2> 
        where T1 : struct 
        where T2 : struct  
    {
        public T1 First { get; }
        public T2 Second { get; }

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }

    public sealed class PairClass<T1, T2>
    {
        public T1 First { get; }
        public T2 Second { get; }

        public PairClass(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PairClass<T1, T2>);
        }

        private bool Equals(PairClass<T1, T2> other)
        {
            if ((object)this == (object)other)
                return true;

            if ((object)other == null)
                return false;

            if (this.GetType() != other.GetType())
                return false;

            return EqualsHelper(this, other);
        }

        private static bool EqualsHelper(PairClass<T1, T2> left, PairClass<T1, T2> right)
        {
            return left.First.Equals(right.First) && left.Second.Equals(right.Second);
        }
    }

    public static class GatherPairs
    {
        // N * log N
        public static List<Pair<int, int>> CollectPairs(ICollection<int> search, int x)
        {
            var data = search.OrderBy(i => i).ToArray();
            var result = new List<Pair<int, int>>();
            var usedArgs = new HashSet<int>();

            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] > x) break;
                if (i > 0 && data[i - 1] == data[i]) continue;

                var other = x - data[i];
                if (usedArgs.Contains(other)) continue;

                var index = Array.BinarySearch(data, i + 1, data.Length - i - 1, other);

                if (index >= 0)
                {
                    result.Add(new Pair<int, int>(data[i], data[index]));
                    usedArgs.Add(data[i]);
                    usedArgs.Add(data[index]);
                }
            }

            return result;
        }

        // N^2 
        public static List<Pair<int, int>> CollectPairsNaive(IList<int> search, int x)
        {
            List<Pair<int, int>> result = new List<Pair<int, int>>();

            for (var i = 0; i < search.Count; i++)
            {
                for (int j = i + 1; j < search.Count; j++)
                {
                    if (search[i] + search[j] == x)
                    {
                        result.Add(new Pair<int, int>(search[i], search[j]));
                    }

                }
            }

            return result.GroupBy(pair => pair.First, pair => pair)
                         .Select(group => group.First())
                         .ToList();
        }


        // N * log N
        public static List<PairClass<int, int>> CollectPairsClass(ICollection<int> search, int x)
        {
            var data = search.OrderBy(i => i).ToArray();
            var result = new List<PairClass<int, int>>();
            var usedArgs = new HashSet<int>();

            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] > x) break;
                if (i > 0 && data[i - 1] == data[i]) continue;

                var other = x - data[i];
                if (usedArgs.Contains(other)) continue;

                var index = Array.BinarySearch(data, i + 1, data.Length - i - 1, other);

                if (index >= 0)
                {
                    result.Add(new PairClass<int, int>(data[i], data[index]));
                    usedArgs.Add(data[i]);
                    usedArgs.Add(data[index]);
                }
            }

            return result;
        }
    }
}
