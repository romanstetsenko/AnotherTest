using System;
using System.Collections.Generic;

namespace ConsoleApp1.Extensions
{
    public static class EnumerableEx
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var s in source) action(s);
        }
    }
}