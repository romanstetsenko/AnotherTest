using ConsoleApp1.Data;

namespace ConsoleApp1.DataSources.Prices
{
    public class WebService2 : IPriceDataSource
    {
        public static Price[] GetPricesFromWebService2()
        {
            // may take 5-10 seconds

            return Generators.GeneratePrices();
        }
        public Price[] GetPrices() => GetPricesFromWebService2();
    }
}