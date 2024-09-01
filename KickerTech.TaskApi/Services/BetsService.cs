using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public class BetsService
    {
        private readonly string _filePath;

        public BetsService()
        {
            _filePath = "/Data/Bets.json";
        }

        public List<Bet> GetBets()
        {
            var jsonString = File.ReadAllText(_filePath);
            var bets = JsonSerializer.Deserialize<List<Bet>>(jsonString);

            return bets ?? new List<Bet>();
        }
    }
}
