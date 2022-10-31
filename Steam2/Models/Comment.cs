namespace Steam2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Profile Author { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public int Rating { get; set; }
        public DateTime PublishingDate { get; set; }
        public bool Edited { get; set; }
        public Game AffectedGame { get; set; }

        public Comment()
        {

        }
    }
}
