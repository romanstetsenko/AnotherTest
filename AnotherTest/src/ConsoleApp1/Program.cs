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
        private static void Main()
        {
            var pricesAsync = PriceService.GetPricesAsync();
            var positionsAsync = PositionService.GetPositionsAsync();
            var (pricesResult, positionsResult) = TaskEx.WhenAll(pricesAsync, positionsAsync).Result;

            if (pricesResult is Success<Price[]> pricesSuccess &&
                positionsResult is Success<Position[]> positionsSuccess)
            {
                var marketValuesResult =
                    MarketValueService.CreateMarketValues(pricesSuccess.Value, positionsSuccess.Value);

                if (marketValuesResult is Success<IEnumerable<MarketValue>> marketValuesSuccess)
                {
                    var marketValues = marketValuesSuccess.Value;
                    marketValues.Select(MarketValue.ToFormatString).ForEach(Console.WriteLine);
                }
                else
                {
                    WriteLineIfFailure(marketValuesResult);
                }
            }
            else
            {
                WriteLineIfFailure(pricesResult);
                WriteLineIfFailure(positionsResult);
            }

            Console.ReadLine();
        }

        private static void WriteLineIfFailure<TValue>(Result<TValue> result)
        {
            if (result is Failure<TValue> failure)
            {
                Console.WriteLine(failure.Reason);
            }
        }
    }
}