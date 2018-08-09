using System;
using System.Linq;
using ConsoleApp1.Data;
using ConsoleApp1.Extensions;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    internal static class Program
    {
        private static void Main()
        {
            var pricesAsync = PriceService.GetPricesAsync();
            var positionsAsync = PositionService.GetPositionsAsync();
            var (pricesResult, positionsResult) = TaskEx.WhenAll(pricesAsync, positionsAsync).Result;

            if (pricesResult is Success<Price[]> pricesSuccess && positionsResult is Success<Position[]> positionsSuccess)
            {
                var marketValues =
                    MarketValueService.CreateMarketValues(pricesSuccess.Value, positionsSuccess.Value);
                marketValues.Select(MarketValue.ToFormatString).ForEach(Console.WriteLine);
            }
            else
            {
                if(pricesResult is Failure pricesFailure)
                    Console.WriteLine(pricesFailure.Reason);
                if (positionsResult is Failure positionsFailure)
                    Console.WriteLine(positionsFailure.Reason);
            }

            Console.ReadLine();
        }
    }
}