using System;
using System.Threading;
using ConsoleApp1.Data;

namespace ConsoleApp1.DataSources
{
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
            var r = new Random();
            Thread.Sleep(r.Next(1, 5) * 1000);

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