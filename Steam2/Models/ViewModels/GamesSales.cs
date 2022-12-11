namespace Steam2.Models.ViewModels
{
    public class GamesSales
    {
        public List<Game> Games { get; set; }
        public List<Sales> Sales { get; set; }
        public string ProfileId { get; set; }

        public GamesSales(List<Game> InputGames, List<Sales> InputSales, string InputProfileId)
        {
            Games = InputGames;
            Sales = InputSales;
            ProfileId = InputProfileId;
        }
    }
}
