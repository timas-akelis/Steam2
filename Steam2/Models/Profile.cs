namespace Steam2.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Context { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public int ConnectedUsers { get; set; }

        public Profile()
        {

        }

        public bool Exists()
        {
            return this != null;
        }
    }
}
