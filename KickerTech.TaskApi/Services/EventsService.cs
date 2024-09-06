using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public interface IEventsService
    {
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

        
    }
}
