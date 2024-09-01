namespace KickerTech.TaskApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public bool IsLive { get; set; }
        public DateTime StartDate { get; set; }
        public List<Odd> Odds { get; set; }
    }
}
