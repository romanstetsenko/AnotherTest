using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    /// <summary>
    ///     Market price of certain product at certain date
    /// </summary>
    public class Price
    {
        public DateTime Date { get; set; }
        public string ProductKey { get; set; }
        public double Value { get; set; }

        public static IEqualityComparer<Price> DateProductKeyComparer { get; } = new DateProductKeyEqualityComparer();

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(ProductKey)}: {ProductKey}, {nameof(Value)}: {Value}";
        }

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


    public struct PricePositionJunction
    {
        public PricePositionJunction(Position position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));
            Date = position.Date;
            ProductKey = position.ProductKey;
        }

        public string ProductKey { get; }

        public DateTime Date { get; }

        public PricePositionJunction(Price price)
        {
            if (price == null) throw new ArgumentNullException(nameof(price));
            Date = price.Date;
            ProductKey = price.ProductKey;
        }
    }

    /// <summary>
    ///     Represents amount of certain product that was present in client's portfolio at certain date
    /// </summary>
    public class Position
    {
        public int PositionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string ProductKey { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(PositionId)}: {PositionId}, {nameof(Date)}: {Date}, {nameof(Amount)}: {Amount}, {nameof(ProductKey)}: {ProductKey}";
        }
    }

    public class MarketValue
    {
        public MarketValue(Position position, Price price)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Value = position.Amount * (decimal) price.Value;
        }

        public decimal Value { get; }
        public Position Position { get; }
        public Price Price { get; }

        public override string ToString()
        {
            return
                $"Market value of position {Position.PositionId} is {Value} for {Position.ProductKey} on date {Price.Date.ToShortDateString()}";
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Price[] prices = GetPrices(); // can be more than 100000 records
            Position[] positions = GetPositions(); // can be more than 1000 records

            var marketValues =
                positions
                    .Join(prices,
                        position => new PricePositionJunction(position),
                        price => new PricePositionJunction(price),
                        (position, price) => new MarketValue(position, price));

            foreach (var marketValue in marketValues) Console.WriteLine(marketValue);

            Console.ReadLine();
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
                new PricesFromWebService1(),
                new PricesFromWebService2(),
                new PricesFromWebService3()
            };
            return GetPrices(dataSources);
        }
    }


    public interface IPriceDataSource
    {
        Price[] GetPrices();
    }

    public class PricesFromWebService1: IPriceDataSource
    {
        public static Price[] GetPricesFromWebService1()
        {
            // may take 5-10 seconds
            return Generators.GeneratePrices();
        }

        public Price[] GetPrices() => GetPricesFromWebService1();
    }

    public class PricesFromWebService2 : IPriceDataSource
    {
        public static Price[] GetPricesFromWebService2()
        {
            // may take 5-10 seconds

            return Generators.GeneratePrices();
        }
        public Price[] GetPrices() => GetPricesFromWebService2();
    }

    public class PricesFromWebService3 : IPriceDataSource
    {
        public Price[] GetPricesFromWebService3()
        {
            // may take 5-10 seconds

            return Generators.GeneratePrices();
        }

        public Price[] GetPrices() => GetPricesFromWebService3();
    }

    public class Generators
    {
        public static Price[] GeneratePrices()
        {
            var r = new Random();
            Thread.Sleep(r.Next(5, 10) * 1000);

            var price1 = new Price {Date = new DateTime(2018, 01, 01), ProductKey = "AAA", Value = 0.1};
            var price2 = new Price {Date = new DateTime(2018, 01, 02), ProductKey = "AAA", Value = 0.2};
            var price3 = new Price {Date = new DateTime(2018, 02, 01), ProductKey = "AAA", Value = 0.3};
            var price4 = new Price {Date = new DateTime(2018, 02, 01), ProductKey = "BBB", Value = 1.1};
            var price5 = new Price {Date = new DateTime(2018, 02, 01), ProductKey = "CCC", Value = 100};
            
            return new[] {price1, price2, price3, price4, price5};
        }

        public static Position[] GeneratePositions()
        {
            var position1 = new Position
            {
                ProductKey = "AAA",
                Amount = 1.5m,
                Date = new DateTime(2018, 01, 02),
                PositionId = 1
            };
            var position2 = new Position
            {
                ProductKey = "BBB",
                Amount = 3,
                Date = new DateTime(2018, 02, 01),
                PositionId = 2
            };
            return new[] {position1, position2};
        }
    }
}