using WeddingProj.Models;
using Microsoft.EntityFrameworkCore;

namespace WeddingProj.Models
{
    public class WeddingContext : DbContext
    {
        public WeddingContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<UserWeddingRSVP> RSVP { get; set; }
    }
}