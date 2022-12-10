namespace Steam2.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string GameIDs { get; set; }
        public string ProfileID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Order()
        {

        }

    }
}
