using Microsoft.EntityFrameworkCore;

namespace Steam2.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public String GamesID { get; set; }
        public decimal FullPrice { get; set; }

        public Cart()
        {
            FullPrice = 0;
        }
    }
}
