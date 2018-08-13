using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class MarketValueFactory
    {
        public MarketValue Create(Position position, Price price) => new MarketValue(position, price);
    }
}