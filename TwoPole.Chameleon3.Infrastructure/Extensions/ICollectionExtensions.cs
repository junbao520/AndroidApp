using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class ICollectionExtentions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    source.Add(item);
                }
            }
            return source;
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> source, IEnumerable<T> removed)
        {
            if (removed != null)
                return source.RemoveRange(removed.ToArray());
            return source;
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> source, params T[] parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    source.Remove(parameter);
                }
            }
            return source;
        }
    }
}
