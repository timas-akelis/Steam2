using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Steam2.Models;

namespace Steam2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Steam2.Models.Profile> Profile { get; set; }
        public DbSet<Steam2.Models.Cart> Cart { get; set; }
        public DbSet<Steam2.Models.Game> Game { get; set; }
        public DbSet<Steam2.Models.Wishlist> Wishlist { get; set; }
    }
}