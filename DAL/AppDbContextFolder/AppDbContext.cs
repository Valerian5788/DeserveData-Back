using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.AppDbContextFolder
{
    public class AppDbContext : DbContext
    {
        public DbSet<Station> Stations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure modelBuilder if needed
        }
    }
}
