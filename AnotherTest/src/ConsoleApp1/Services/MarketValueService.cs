using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class MarketValueService
    {
        public static IEnumerable<MarketValue> CreateMarketValues(IEnumerable<Position> positions,
            IEnumerable<Price> prices)
        {
            return positions
                .Join(prices,
                    position => new PricePositionJunction(position),
                    price => new PricePositionJunction(price),
                    (position, price) => new MarketValue(position, price));
        }
    }
}