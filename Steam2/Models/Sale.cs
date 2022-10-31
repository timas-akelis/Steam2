namespace Steam2.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public List<Game> AffectedGames { get; set; }

        public Sale()
        {

        }
    }
}
