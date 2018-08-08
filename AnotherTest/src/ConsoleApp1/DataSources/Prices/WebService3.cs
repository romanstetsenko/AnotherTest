using ConsoleApp1.Data;

namespace ConsoleApp1.DataSources.Prices
{
    public class WebService3 : IPriceDataSource
    {
        public Price[] GetPricesFromWebService3()
        {
            // may take 5-10 seconds

            return Generators.GeneratePrices();
        }

        public Price[] GetPrices() => GetPricesFromWebService3();
    }
}