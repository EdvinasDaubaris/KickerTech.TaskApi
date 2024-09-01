using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public class EventsService
    {
        private readonly string _filePath;

        public EventsService()
        {
            _filePath = "/Data/Events.json";
        }

        public List<Event> GetEvents()
        {
            var jsonString = File.ReadAllText(_filePath);
            var events = JsonSerializer.Deserialize<List<Event>>(jsonString);

            return events ?? new List<Event>();
        }


    }
}
