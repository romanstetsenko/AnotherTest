using ConsoleApp1.Data;
using ConsoleApp1.DataSources;

namespace ConsoleApp1.Services
{
    public class PositionMockService
    {
        public static Position[] GetPositions()
        {
            return Generators.GeneratePositions();
        }
    }
}