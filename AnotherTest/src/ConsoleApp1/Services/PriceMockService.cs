using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;
using ConsoleApp1.DataSources.Prices;

namespace ConsoleApp1.Services
{
    public class PriceMockService
    {
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