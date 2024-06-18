using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Microsoft.IdentityModel.Abstractions;

namespace DAL.AppDbContextFolder
{
    public class AppDbContext : DbContext
    {
        public DbSet<Station> stations_fr { get; set; }
        public DbSet<Facilities> facilities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                .HasKey(s => s.Id_Stations_fr); // Assuming Id_Stations_fr is the primary key in Station

            modelBuilder.Entity<Facilities>()
                .HasKey(f => f.Id_Stations_fr); // Assuming Id_Stations_fr is both the primary key and foreign key in Facilities

            // Configure the one-to-one relationship between Station and Facilities
            modelBuilder.Entity<Station>()
                .HasOne<Facilities>(s => s.Facilities) // Station has one Facilities
                .WithOne(f => f.Station) // Facilities is associated with one Station
                .HasForeignKey<Facilities>(f => f.Id_Stations_fr); // The foreign key in Facilities pointing to Station
        }
    }
}
