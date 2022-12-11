namespace Steam2.Models.ViewModels
{
    public class GameGenreAchievementComment
    {
        public Game Game { get; set; }
        public List<string> ListedGenres { get; set; }
        public Achievement Achievement { get; set; }
        public Comment Comment { get; set; }

        public GameGenreAchievementComment(Game InputGame, List<string> InputListedGenres, Achievement InputAchievement, Comment InputComment)
        {
            Game = InputGame;
            ListedGenres = InputListedGenres;
            Achievement = InputAchievement;
            Comment = InputComment;
        }
    }
}
