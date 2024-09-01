using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public class PlayersService
    {
        private readonly string _filePath;

        public PlayersService()
        {
            _filePath = "/Data/Players.json";
        }

        public List<Player> GetPlayers()
        {
            var jsonString = File.ReadAllText(_filePath);

            var players = JsonSerializer.Deserialize<List<Player>>(jsonString);

            return players ?? new List<Player>();
        }
    }
}
