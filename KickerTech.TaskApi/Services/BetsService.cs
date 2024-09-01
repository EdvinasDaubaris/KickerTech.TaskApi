using KickerTech.TaskApi.Models;
using System.Text.Json;

namespace KickerTech.TaskApi.Services
{
    public interface IBetsService
    {
        List<Bet> GetBets();
        void CreateBet(int playerId, int eventId, int oddId, decimal betSum, ResultCode resultCode);
    }

    public class BetsService : IBetsService
    {
        private readonly string _filePath;

        public BetsService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Bets.json"); ;
        }

        public List<Bet> GetBets()
        {
            var jsonString = File.ReadAllText(_filePath);
            var bets = JsonSerializer.Deserialize<List<Bet>>(jsonString);

            return bets ?? new List<Bet>();
        }

        public void CreateBet(int playerId, int eventId, int oddId, decimal betSum, ResultCode resultCode)
        {
            var bets = GetBets();

            
            var newBet = new Bet
            {
                Id = bets.Any() ? bets.Max(b => b.Id) + 1 : 1, // Auto-increment the Id
                EventId = eventId,
                PlayerId = playerId,
                OddId = oddId,
                ResultCode = resultCode, 
                BetSum = betSum
            };

            bets.Add(newBet);

            var jsonString = JsonSerializer.Serialize(bets, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);

        }
    }
}
