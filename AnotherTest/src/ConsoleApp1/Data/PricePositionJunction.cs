using System;

namespace ConsoleApp1.Data
{
    public struct PricePositionJunction
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