namespace Steam2.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Context { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public int ConnecedUsers { get; set; }

        public Profile()
        {
            State = "not defined";
        }
    }
}
