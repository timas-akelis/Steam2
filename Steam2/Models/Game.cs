namespace Steam2.Models
{
    public class Game
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishingDate { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string SaleId { get; set; }
        public Game()
        {

        }
    }
}
