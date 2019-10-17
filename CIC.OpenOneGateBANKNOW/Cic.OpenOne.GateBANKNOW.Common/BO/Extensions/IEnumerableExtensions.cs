namespace Cic.OpenOne.GateBANKNOW.Common.BO.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable == null || action == null)
            {
                return Enumerable.Empty<T>();
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }

            return enumerable;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector)
        {
            return enumerable
                .GroupBy(keySelector)
                .Select(group => group.First());
        }
    }
}
