using System.Collections.Generic;
using System.Linq;

namespace TSharp.UnitOfWorkGenerator.Core
{
    internal static class ForeachWithIndex
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}