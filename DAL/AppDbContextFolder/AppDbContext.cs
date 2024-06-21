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

            // Configure the one-to-one relationship between Station and Facilities
            modelBuilder.Entity<Station>()
                .HasOne<Facilities>(s => s.Facilities) // Station has one Facilities
                .WithOne(f => f.Station) // Facilities is associated with one Station
                .HasForeignKey<Facilities>(f => f.Id_Station); // The foreign key in Facilities pointing to Station

            modelBuilder.Entity<Platforms>()
                .HasKey(p => p.Perron_Id);

            // Define the relationship between Platforms and Station
            modelBuilder.Entity<Platforms>()
                .HasOne(p => p.Station) // Platforms has one Station
                .WithMany() // Station has many Platforms (or specify the collection if available)
                .HasForeignKey(p => p.Id_Station); // The foreign key

        }
    }
}
