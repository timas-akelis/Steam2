namespace Steam2.Models
{
    public class GamePage
    {
        public Game GameId { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
    }
}
