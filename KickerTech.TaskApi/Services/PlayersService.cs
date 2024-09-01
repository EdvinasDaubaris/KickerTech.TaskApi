using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public interface IPlayersService
    {
        List<Player> GetPlayers();
        void UpdatePlayerBalance(int playerId, decimal newBalance);
    }

    public class PlayersService : IPlayersService
    {
        private readonly string _filePath;
        public PlayersService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Players.json");
        }

        public List<Player> GetPlayers()
        {
            var jsonString = File.ReadAllText(_filePath);

            var players = JsonSerializer.Deserialize<List<Player>>(jsonString);

            return players ?? new List<Player>();
        }

        public void UpdatePlayerBalance(int playerId, decimal newBalance)
        {
            var players = GetPlayers();
            var player = players.FirstOrDefault(p => p.Id == playerId);
            player.Balance = newBalance;

            var jsonString = JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
