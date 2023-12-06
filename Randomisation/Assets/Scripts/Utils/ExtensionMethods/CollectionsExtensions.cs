using System.Collections.Generic;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class CollectionsExtensions
    {
        public static string ToDisplayString<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return "null";

            string s = "[";
            s = source.Aggregate(s, (res, x) => res + x + ", ");

            if (s.Contains(", "))
                s = s.Substring(0, s.Length - 2);

            return $"{s}]";
        }

    }
}