using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;
using ConsoleApp1.Extensions;

namespace ConsoleApp1.Services
{
    public class MarketValueService
    {
        public static Result<IEnumerable<MarketValue>> CreateMarketValues(IEnumerable<Price> prices,
            IEnumerable<Position> positions)
        {
            try
            {
                var marketValues = positions
                    .Join(prices,
                        position => new PricePositionJunction(position),
                        price => new PricePositionJunction(price),
                        (position, price) => new MarketValue(position, price));
                return new Success<IEnumerable<MarketValue>>(marketValues);
            }
            catch (Exception e)
            {
                return new Failure<IEnumerable<MarketValue>>(e.Message);
            }
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