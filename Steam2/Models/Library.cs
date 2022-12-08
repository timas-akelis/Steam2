using Newtonsoft.Json.Serialization;

namespace Steam2.Models
{
    public class Library
    {
        public string Id { get; set; }
        public string GamesID { get; set; }
        public string ProfileID { get; set; }
        public DateTime Date { get; set; }
        public double HoursPlayed { get; set; }
        public double RecentHoursPlayer { get; set; }

        public Library() { }
    }
}
