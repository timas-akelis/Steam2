namespace Steam2.Models.GamesComments
{
    public class GamesComments
    {
        public Game game;
        public List<Comment> comments;

        public GamesComments(Game g, List<Comment> c)
        {
            game = g;
            comments = c;
        }
    }
}
