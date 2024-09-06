using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public interface IOddsService
    {
        decimal GetOddTolerancePercentage(decimal actualOddValue, decimal expectedOddValue);
    }

    public class OddsService : IOddsService
    {
        public OddsService()
        {
        }

        public decimal GetOddTolerancePercentage(decimal actualOddValue,decimal expectedOddValue)
        {
            decimal difference = Math.Abs(actualOddValue - expectedOddValue);
            decimal baseValue = Math.Max(actualOddValue, expectedOddValue);
            return (difference / baseValue) * 100;
        }
    }
}
