using System;
using System.Collections.Generic;
using ConsoleApp1.Data;
using ConsoleApp1.Extensions;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    public abstract class AppFlow
    {
        protected readonly MarketValueCollater MarketValueCollater;
        protected readonly Func<Price[]> GetInputPrices;
        protected readonly Func<Position[]> GetInputPositions;

        protected AppFlow(MarketValueCollater marketValueCollater, Func<Price[]> getInputPrices, Func<Position[]> getInputPositions)
        {
            MarketValueCollater = marketValueCollater;
            GetInputPrices = getInputPrices;
            GetInputPositions = getInputPositions;
        }

        public Result<IEnumerable<MarketValue>> Run()
        {
            var inputsResult = GetInputs();

            if (inputsResult is Failure<(Price[], Position[])> failure)
            {
                return new Failure<IEnumerable<MarketValue>>(failure.Reason);
            }

            if (inputsResult is Success<(Price[], Position[])> success)
            {
                var successValue = success.Value;
                return MarketValueCollater.Collate(successValue.Item1, successValue.Item2);
            }

            return new Failure<IEnumerable<MarketValue>>("Fatal error.");
        }

        protected abstract Result<(Price[], Position[])> GetInputs();
    }

    public class SequentalAppFlow : AppFlow
    {
        public SequentalAppFlow(
            MarketValueCollater marketValueCollater,
            Func<Price[]> getInputPrices,
            Func<Position[]> getInputPositions)
            : base(marketValueCollater, getInputPrices, getInputPositions)
        {
        }

        protected override Result<(Price[], Position[])> GetInputs()
        {
            var pricesResult = ResultEx.TryCatch(GetInputPrices);
            if (pricesResult is Failure<Price[]> getPricesFail)
            {
                return new Failure<(Price[], Position[])>(getPricesFail.Reason);
            }

            var positionsResult = ResultEx.TryCatch(GetInputPositions);
            if (positionsResult is Failure<Position[]> getPostionsFail)
            {
                return new Failure<(Price[], Position[])>(getPostionsFail.Reason);
            }

            // finally we succeeded
            if (pricesResult is Success<Price[]> pricesSuccess &&
                positionsResult is Success<Position[]> positionsSucces)
            {
                return new Success<(Price[], Position[])>((pricesSuccess.Value, positionsSucces.Value));
            }

            return new Failure<(Price[], Position[])>("SequentalAppFlow.GetInputs - Fatal error.");
        }
    }

    public class ParallelAppFlow : AppFlow
    {
        public ParallelAppFlow(
            MarketValueCollater marketValueCollater,
            Func<Price[]> getInputPrices,
            Func<Position[]> getInputPositions)
            : base(marketValueCollater, getInputPrices, getInputPositions)
        {
        }

        protected override Result<(Price[], Position[])> GetInputs()
        {
            var pricesAsync = ResultEx.TryCatchAsync(GetInputPrices);
            var positionsAsync = ResultEx.TryCatchAsync(GetInputPositions);
            var (pricesResult, positionsResult) = TaskEx.WhenAll(pricesAsync, positionsAsync).Result;
            switch (pricesResult)
            {
                case Success<Price[]> successPrice when positionsResult is Success<Position[]> positionsSuccess:
                    return new Success<(Price[], Position[])>((successPrice.Value, positionsSuccess.Value));

                // :( it's all about failures
                case Failure<Price[]> priceFailure when positionsResult is Failure<Position[]> positionFailure:
                    return new Failure<(Price[], Position[])>(priceFailure.Reason + " " + positionFailure.Reason);
                case Failure<Price[]> failure:
                    return new Failure<(Price[], Position[])>(failure.Reason);
                case Success<Price[]> _ when positionsResult is Failure<Position[]> failure:
                    return new Failure<(Price[], Position[])>(failure.Reason);
            }

            return new Failure<(Price[], Position[])>("ParallelAppFlow.GetInputs - Fatal error.");
        }
    }
}