using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public interface IEventsService
    {
        decimal CalculateOddPercentage(Event eventObj, int oddId);
        List<Event> GetEvents();
    }

    public class EventsService : IEventsService
    {
        private readonly string _filePath;

        public EventsService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Events.json");
        }

        public List<Event> GetEvents()
        {
            var jsonString = File.ReadAllText(_filePath);
            var events = JsonSerializer.Deserialize<List<Event>>(jsonString);

            return events ?? new List<Event>();
        }

        public decimal CalculateOddPercentage(Event eventObj, int oddId)
        {
            var odd = eventObj.Odds.FirstOrDefault(o => o.Id == oddId);

            decimal totalValue = eventObj.Odds.Sum(o => o.Value);

            if (totalValue == 0)
            {
                throw new InvalidOperationException("The total value of all odds is zero, cannot calculate percentage.");
            }

            decimal percentage = (decimal)((odd.Value / totalValue) * 100);

            return percentage;
        }
    }
}
