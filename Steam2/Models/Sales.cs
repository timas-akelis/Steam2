namespace Steam2.Models
{
    public class Sales
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Amount { get; set; }
        public string Specification { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }

        public Sales()
        {

        }
    }
}
