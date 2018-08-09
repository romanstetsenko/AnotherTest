namespace ConsoleApp1.Extensions
{
    public class Result
    {
    }

    public class Failure : Result
    {
        public Failure(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    public class Success<TValue> : Result
    {
        public Success(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}