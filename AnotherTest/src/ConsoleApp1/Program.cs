using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;
using ConsoleApp1.Extensions;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // define your inputs
            Price[] GetInputPrices()
            {
                return PriceMockService.GetPrices();
            }

            Position[] GetInputPositions()
            {
                return PositionMockService.GetPositions();
            }

            // set output result creator
            var marketValueFactory = new MarketValueFactory();

            // specify a joining algorithm
            var marketValueCollater = new MarketValueCollater(marketValueFactory);

            // choose approach to execute the logic
            var appFlow = new ParallelAppFlow(marketValueCollater, GetInputPrices, GetInputPositions);
            //var appFlow = new SequentalAppFlow(marketValueCollater, GetInputPrices, GetInputPositions);

            var appResult = appFlow.Run();

            PrintResult(appResult);

            Console.ReadLine();
        }

        private static void PrintResult(Result<IEnumerable<MarketValue>> appResult)
        {
            switch (appResult)
            {
                case Success<IEnumerable<MarketValue>> success:
                    success.Value.Select(MarketValue.ToFormatString).ForEach(Console.WriteLine);
                    break;

                case Failure<IEnumerable<MarketValue>> failure:
                    Console.WriteLine(failure.Reason);
                    break;
            }
        }
    }
}