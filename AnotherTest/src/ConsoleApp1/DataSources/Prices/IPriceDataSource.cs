using ConsoleApp1.Data;

namespace ConsoleApp1.DataSources.Prices
{
    public interface IPriceDataSource
    {
        Price[] GetPrices();
    }
}