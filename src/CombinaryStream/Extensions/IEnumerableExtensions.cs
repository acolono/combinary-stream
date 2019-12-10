using System.Collections.Generic;
using System.Linq;

namespace CombinaryStream.Extensions {
    public static class IEnumerableExtensions {
        public static IEnumerable<T> SkipTake<T>(this IEnumerable<T> items, int? offset, int? limit) {
            if (offset.HasValue && offset.Value > 0) items = items.Skip(offset.Value);
            if (limit.HasValue && limit.Value > 0) items = items.Take(limit.Value);
            return items;
        }
    }
}