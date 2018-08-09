using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class MarketValueService
    {
        public static IEnumerable<MarketValue> CreateMarketValues(IEnumerable<Price> prices,
            IEnumerable<Position> positions)
        {
            return positions
                .Join(prices,
                    position => new PricePositionJunction(position),
                    price => new PricePositionJunction(price),
                    (position, price) => new MarketValue(position, price));
        }

        internal struct PricePositionJunction
        {
            public PricePositionJunction(Position position)
            {
                if (position == null) throw new ArgumentNullException(nameof(position));
                Date = position.Date;
                ProductKey = position.ProductKey;
            }

            public string ProductKey { get; }

            public DateTime Date { get; }

            public PricePositionJunction(Price price)
            {
                if (price == null) throw new ArgumentNullException(nameof(price));
                Date = price.Date;
                ProductKey = price.ProductKey;
            }
        }
    }
}