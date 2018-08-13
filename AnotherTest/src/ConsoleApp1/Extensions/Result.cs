using System;
using System.Threading.Tasks;

namespace ConsoleApp1.Extensions
{
    public class Result<TSuccess>
    {
    }

    public class Failure<TSuccess> : Result<TSuccess>
    {
        public Failure(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    public class Success<TSuccess> : Result<TSuccess>
    {
        public Success(TSuccess value)
        {
            Value = value;
        }

        public TSuccess Value { get; }
    }

    public class ResultEx
    {
        public static Result<T> TryCatch<T>(Func<T> fun)
        {
            try
            {
                var value = fun();
                return new Success<T>(value);
            }
            catch (Exception e)
            {
                return new Failure<T>(e.Message);
            }
        }
        public static Task<Result<T>> TryCatchAsync<T>(Func<T> fun)
        {
            return Task.Run(() => TryCatch(fun));
        }
    }
}