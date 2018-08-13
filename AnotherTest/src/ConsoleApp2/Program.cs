using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        public static async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2)
        {
            await Task.WhenAll(task1, task2);
            return (task1.Result, task2.Result);
        }

        private static void Main()
        {
            var getPricesAsync = Task.Run(() => GetPrices());
            var getPositionsAsync = Task.Run(() => GetPositions());
            var (positions, prices) = WhenAll(getPositionsAsync, getPricesAsync).Result;

            string CreateOutput(Position position, Price price) =>
                $"Market value of position {position.PositionId} is {position.Amount * (decimal) price.Value} for {position.ProductKey} on date {position.Date.ToShortDateString()}";

            foreach (var marketValueOutput in Collate(positions, prices, CreateOutput))
                Console.WriteLine(marketValueOutput);

            Console.ReadKey();
        }

        private static Position[] GetPositions()
        {
            var position1 = new Position
            {
                ProductKey = "AAA",
                Amount = 1.5m,
                Date = new DateTime(2018, 01, 01),
                PositionId = 1
            };
            return new[] {position1};
        }

        private static Price[] GetPrices()
        {
            var dataSources = new Func<Price[]>[]
            {
                GetPricesFromWebService1,
                GetPricesFromWebService2,
                GetPricesFromWebService3
            };
            var tasks = dataSources.Select(Task.Run);
            var manyPrices = Task.WhenAll(tasks).Result;

            return manyPrices
                .AsParallel().AsUnordered()
                .SelectMany(prices => prices)
                .Distinct(Price.DateProductKeyComparer).ToArray();
        }

        public static Price[] GetPricesFromWebService1()
        {
            // may take 5-10 seconds
            var price1 = new Price {Date = new DateTime(2018, 01, 01), ProductKey = "AAA", Value = 0.1};
            return new[] {price1};
        }

        public static Price[] GetPricesFromWebService2()

        {
            // may take 5-10 seconds
            var price1 = new Price {Date = new DateTime(2018, 01, 01), ProductKey = "AAA", Value = 0.1};
            return new[] {price1};
        }

        public static Price[] GetPricesFromWebService3()

        {
            // may take 5-10 seconds
            return new Price[] { };
        }

        public static IEnumerable<TMarketValue> Collate<TMarketValue>(
            IEnumerable<Position> positions,
            IEnumerable<Price> prices,
            Func<Position, Price, TMarketValue> createMarketValue)
        {
            return positions
                .Join(prices,
                    position => (position.ProductKey, position.Date),
                    price => (price.ProductKey, price.Date),
                    (position, price) => createMarketValue(position, price));
        }

        public class Price
        {
            public DateTime Date { get; set; }
            public string ProductKey { get; set; }
            public double Value { get; set; }

            public static IEqualityComparer<Price> DateProductKeyComparer { get; } =
                new DateProductKeyEqualityComparer();

            private sealed class DateProductKeyEqualityComparer : IEqualityComparer<Price>
            {
                public bool Equals(Price x, Price y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (x is null) return false;
                    if (y is null) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.Date.Equals(y.Date) &&
                           string.Equals(x.ProductKey, y.ProductKey, StringComparison.InvariantCultureIgnoreCase);
                }

                public int GetHashCode(Price obj)
                {
                    unchecked
                    {
                        return (obj.Date.GetHashCode() * 397) ^ (obj.ProductKey != null
                                   ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.ProductKey)
                                   : 0);
                    }
                }
            }
        }

        public class Position
        {
            public int PositionId { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string ProductKey { get; set; }
        }
    }
}