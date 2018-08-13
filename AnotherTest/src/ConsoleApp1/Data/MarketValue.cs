using System;

namespace ConsoleApp1.Data
{
    /// <summary>
    /// market value of position (equals to Price.Value * Position.Amount) on a given date
    /// </summary>
    public class MarketValue
    {
        public MarketValue(Position position, Price price)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (price == null) throw new ArgumentNullException(nameof(price));
            Value = position.Amount * (decimal) price.Value;
            PositionId = position.PositionId;
            ProductKey = position.ProductKey;
            Date = price.Date;
        }

        public DateTime Date { get;}

        public string ProductKey { get;}

        public int PositionId { get;}

        public decimal Value { get; }

        public static string ToFormatString(MarketValue marketValue)
        {
            return
                $"Market value of position {marketValue.PositionId} is {marketValue.Value} for {marketValue.ProductKey} on date {marketValue.Date.ToShortDateString()}";
        }
    }
}