namespace Steam2.Models.ViewModels
{
    public class GameGenreAchievementComment
    {
        public Game Game { get; set; }
        public List<string> ListedGenres { get; set; }
        public List<Achievement> Achievement { get; set; }
        public List<Comment> Comment { get; set; }

        public GameGenreAchievementComment(Game InputGame, List<string> InputListedGenres, List<Achievement> InputAchievement, List<Comment> InputComment)
        {
            Game = InputGame;
            ListedGenres = InputListedGenres;
            Achievement = InputAchievement;
            Comment = InputComment;
        }
    }
}
