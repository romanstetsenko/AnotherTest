﻿using System;
using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.DataSources;
using ConsoleApp1.Extensions;

namespace ConsoleApp1.Services
{
    public class PositionService
    {
        public static Position[] GetPositions()
        {
            return Generators.GeneratePositions();
        }

        public static Task<Result<Position[]>> GetPositionsAsync()
        {
            return Task.Run<Result<Position[]>>(() =>
            {
                try
                {
                    var positions = GetPositions();
                    return new Success<Position[]>(positions);
                }
                catch (Exception e)
                {
                    return new Failure<Position[]>(e.Message);
                }
            });
        }
    }
}