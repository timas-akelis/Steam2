using System.Linq.Expressions;

namespace Steam2.Models
{
    public class Achievement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GamesID { get; set; }
        public string ProfileID { get; set; }
        public string Icon { get; set; }
        public DateTime Achieved { get; set; }
        public bool Acquired { get; set; }
        public double PlayerPerc { get; set; }
        public Achievement()
        {

        }

    }
}
