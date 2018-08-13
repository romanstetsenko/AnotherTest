using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Data;
using ConsoleApp1.Extensions;

namespace ConsoleApp1.Services
{
    public class MarketValueCollater
    {
        private readonly MarketValueFactory _marketValueFactory;

        public MarketValueCollater(MarketValueFactory marketValueFactory)
        {
            _marketValueFactory = marketValueFactory;
        }

        public Result<IEnumerable<MarketValue>> Collate(IEnumerable<Price> prices,
            IEnumerable<Position> positions)
        {
            try
            {
                var marketValues = positions
                    .Join(prices,
                        position => (position.ProductKey, position.Date),
                        price => (price.ProductKey, price.Date),
                        (position, price) => _marketValueFactory.Create(position, price));
                return new Success<IEnumerable<MarketValue>>(marketValues);
            }
            catch (Exception e)
            {
                return new Failure<IEnumerable<MarketValue>>(e.Message);
            }
        }
    }
}