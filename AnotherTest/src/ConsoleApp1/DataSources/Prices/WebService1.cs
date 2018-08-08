using ConsoleApp1.Data;

namespace ConsoleApp1.DataSources.Prices
{
    public class WebService1: IPriceDataSource
    {
        public static Price[] GetPricesFromWebService1()
        {
            // may take 5-10 seconds
            return Generators.GeneratePrices();
        }

        public Price[] GetPrices() => GetPricesFromWebService1();
    }
}