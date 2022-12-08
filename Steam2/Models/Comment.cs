namespace Steam2.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string GamesID { get; set; }
        public string ProfileID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public bool Edited { get; set; }
        public Comment()
        {

        }
    }
}
