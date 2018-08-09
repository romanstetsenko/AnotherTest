using System;
using System.Collections.Generic;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    internal static class Program
    {
        private static void Main(string[] _)
        {
            var pricesAsync = PriceService.GetPricesAsync();
            var positionsAsync = PositionService.GetPositionsAsync();
            var (prices, positions) = TaskEx.WhenAll(pricesAsync, positionsAsync).Result;

            var marketValues = MarketValueService.CreateMarketValues(positions, prices);

            marketValues.ForEach(Console.WriteLine);

            Console.ReadLine();
        }

        private static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var s in source) action(s);
        }
    }
}