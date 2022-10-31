namespace Steam2.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishingDate { get; set; }
        public double HoursPlayed { get; set; }
        public double RecentHoursPlayer { get; set; }
    }
}
