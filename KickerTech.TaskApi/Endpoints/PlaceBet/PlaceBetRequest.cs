namespace KickerTech.TaskApi.Endpoints.PlaceBet
{
    public class PlaceBetRequest
    {
        public int PlayerId { get; set; }
        public int EventId { get; set; }
        public int OddId { get; set; }
        public decimal BetSum { get; set; }
    }
}
