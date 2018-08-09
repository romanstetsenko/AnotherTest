using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.DataSources.Prices;
using ConsoleApp1.Extensions;

namespace ConsoleApp1.Services
{
    public class PriceService
    {
        public static Task<Result> GetPricesAsync()
        {
            return Task.Run<Result>(() =>
            {
                try
                {
                    var prices = GetPrices();
                    return new Success<Price[]>(prices);
                }
                catch (Exception e)
                {
                    return new Failure(e.Message);
                }
            });
        }

        public static Price[] GetPrices(IEnumerable<IPriceDataSource> dataSources)
        {
            return dataSources
                .AsParallel().AsUnordered().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(dataSource => dataSource.GetPrices())
                .SelectMany(prices => prices)
                .Distinct(Price.DateProductKeyComparer).ToArray();
        }

        public static Price[] GetPrices()
        {
            var dataSources = new IPriceDataSource[]
            {
                new WebService1(),
                new WebService2(),
                new WebService3()
            };
            return GetPrices(dataSources);
        }
    }
}