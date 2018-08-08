using System;

namespace ConsoleApp1.Data
{
    /// <summary>
    ///     Represents amount of certain product that was present in client's portfolio at certain date
    /// </summary>
    public class Position
    {
        public int PositionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string ProductKey { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(PositionId)}: {PositionId}, {nameof(Date)}: {Date}, {nameof(Amount)}: {Amount}, {nameof(ProductKey)}: {ProductKey}";
        }
    }
}