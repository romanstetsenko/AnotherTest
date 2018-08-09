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
                        position => (position.ProductKey, position.Date),
                        price => (price.ProductKey, price.Date),
                        (position, price) => new MarketValue(position, price));
                return new Success<IEnumerable<MarketValue>>(marketValues);
            }
            catch (Exception e)
            {
                return new Failure<IEnumerable<MarketValue>>(e.Message);
            }
        }
    }
}