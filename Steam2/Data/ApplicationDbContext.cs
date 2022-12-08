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
        public DbSet<Steam2.Models.Achievement> Achievement { get; set; }
        public DbSet<Steam2.Models.Comment> Comment { get; set; }
        public DbSet<Steam2.Models.Genre> Genre { get; set; }
        public DbSet<Steam2.Models.Library> Library { get; set; }
        public DbSet<Steam2.Models.Order> Order { get; set; }
        public DbSet<Steam2.Models.Sales> Sales { get; set; }
    }
}