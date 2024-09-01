namespace KickerTech.TaskApi.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int OddId { get; set; }
        public ResultCode ResultCode { get; set; }
    }

    public enum ResultCode
    {
        Success,
        Failed
    }
}
