namespace ConsoleApp1.Extensions
{
    public class Result<TValue>
    {
    }

    public class Failure<TValue> : Result<TValue>
    {
        public Failure(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    public class Success<TValue> : Result<TValue>
    {
        public Success(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}