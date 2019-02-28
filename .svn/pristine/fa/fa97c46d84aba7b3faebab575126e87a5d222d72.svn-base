using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation
{
    public static class EnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            if (source == null)
                return 0;

            var collection = source as ICollection;
            if (collection != null)
                return collection.Count;

            return Enumerable.Count(source.Cast<object>());
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> func)
        {
            if (source == null)
                return -1;

            int index = 0;
            foreach (var item in source)
            {
                if (func(item))
                    return index;
                index++;
            }
            return -1;
        }

        public static int ElementIndex<T>(this IEnumerable<T> source, T target)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (Equals(item, target))
                    return index;

                index++;
            }
            return -1;
        }

        public static IEnumerable<T> UnionAll<T>(this IEnumerable<T> source, params T[] items)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            foreach (var item in items)
            {
                yield return item;
            }
        }

        public static TResult MinEx<T, TCompareValue, TResult>(this IEnumerable<T> source,
            Func<T, TCompareValue> compareSelector, Func<T, TResult> selector)
            where TCompareValue : IComparable
        {
            T minItem = source.First();
            TCompareValue minCompare = compareSelector(minItem);
            var comparer = Comparer<TCompareValue>.Default;
            foreach (var item in source.Skip(1))
            {
                var c = compareSelector(item);
                var v = comparer.Compare(minCompare, c);
                if (v > 0)
                {
                    minCompare = c;
                    minItem = item;
                }
            }
            return selector(minItem);
        }
    }
}
