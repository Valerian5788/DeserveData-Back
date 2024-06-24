using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Microsoft.IdentityModel.Abstractions;

namespace DAL.AppDbContextFolder
{
    public class AppDbContext : DbContext
    {
        public DbSet<Station> stations { get; set; }
        public DbSet<Facilities> facilities { get; set; }

        public DbSet<BusStop> busStop { get; set; }

        public DbSet<Platforms> platforms { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                .HasKey(s => s.Id_Station);

            modelBuilder.Entity<Facilities>()
                .HasKey(f => f.Id_Station);

            modelBuilder.Entity<BusStop>()
                .HasKey(b => b.StopId);

            modelBuilder.Entity<Platforms>()
                .HasKey(p => p.Perron_Id);

            // Configure the one-to-one relationship between Station and Facilities
            modelBuilder.Entity<Station>()
                .HasOne(s => s.Facilities) // Station has one Facilities
                .WithOne(f => f.Station) // Facilities has one Station
                .HasForeignKey<Facilities>(f => f.Id_Station); // Facilities contains the foreign key

            modelBuilder.Entity<Station>()
                .HasMany(p => p.Platforms) // Platforms has one Station
                .WithOne() // Station has many Platforms (or you can specify the navigation property if available)
                .HasForeignKey(p => p.Id_Station) // Use Id_Station as the foreign key
                .IsRequired(); // Make the foreign key required if it should not be nullable

        }
    }
}
