using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.DataSources;
using ConsoleApp1.DataSources.Prices;

namespace ConsoleApp1
{
    internal static class Program
    {
        private static void Main(string[] _)
        {
            var pricesAsync = Task.Run(() => GetPrices());
            var positionsAsync = Task.Run(() => GetPositions());
            var (prices, positions) = TaskEx.WhenAll(pricesAsync, positionsAsync).Result;

            IEnumerable<MarketValue> marketValues = CreateMarketValues(positions, prices);
            marketValues.ForEach(Console.WriteLine);
            Console.ReadLine();
        }

        private static IEnumerable<MarketValue> CreateMarketValues(IEnumerable<Position> positions, IEnumerable<Price> prices)
        {
            return positions
                .Join(prices,
                    position => new PricePositionJunction(position),
                    price => new PricePositionJunction(price),
                    (position, price) => new MarketValue(position, price));
        }

        private static Position[] GetPositions()
        {
            return Generators.GeneratePositions();
        }

        private static Price[] GetPrices(IEnumerable<IPriceDataSource> dataSources)
        {
            return dataSources
                .AsParallel().AsUnordered().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(dataSource => dataSource.GetPrices())
                .SelectMany(prices => prices)
                .Distinct(Price.DateProductKeyComparer).ToArray();
        }

        private static Price[] GetPrices()
        {
            var dataSources = new IPriceDataSource[]
            {
                new WebService1(),
                new WebService2(),
                new WebService3()
            };
            return GetPrices(dataSources);
        }

        private static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var s in source) action(s);
        }
    }
}