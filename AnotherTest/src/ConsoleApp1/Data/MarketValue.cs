using System;

namespace ConsoleApp1.Data
{
    public class MarketValue
    {
        public MarketValue(Position position, Price price)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Value = position.Amount * (decimal) price.Value;
        }

        public decimal Value { get; }
        public Position Position { get; }
        public Price Price { get; }

        public override string ToString()
        {
            return
                $"Market value of position {Position.PositionId} is {Value} for {Position.ProductKey} on date {Price.Date.ToShortDateString()}";
        }
    }
}