using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.DataSources;

namespace ConsoleApp1.Services
{
    public class PositionService
    {
        public static Task<Position[]> GetPositionsAsync()
        {
            return Task.Run(() => GetPositions());
        }

        public static Position[] GetPositions()
        {
            return Generators.GeneratePositions();
        }
    }
}