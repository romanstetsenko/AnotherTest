using System;
using System.Collections.Generic;

namespace ConsoleApp1.Data
{
    /// <summary>
    ///     Market price of certain product at certain date
    /// </summary>
    public class Price
    {
        public DateTime Date { get; set; }
        public string ProductKey { get; set; }
        public double Value { get; set; }

        public static IEqualityComparer<Price> DateProductKeyComparer { get; } = new DateProductKeyEqualityComparer();

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(ProductKey)}: {ProductKey}, {nameof(Value)}: {Value}";
        }

        private sealed class DateProductKeyEqualityComparer : IEqualityComparer<Price>
        {
            public bool Equals(Price x, Price y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Date.Equals(y.Date) &&
                       string.Equals(x.ProductKey, y.ProductKey, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(Price obj)
            {
                unchecked
                {
                    return (obj.Date.GetHashCode() * 397) ^ (obj.ProductKey != null
                               ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.ProductKey)
                               : 0);
                }
            }
        }
    }
}