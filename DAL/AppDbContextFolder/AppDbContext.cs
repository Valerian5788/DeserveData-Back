using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.AppDbContextFolder
{
    public class AppDbContext : DbContext
    {
        public DbSet<Station> stations { get; set; }
        public DbSet<Facilities> facilities { get; set; }
        public DbSet<BusStop> busStop { get; set; }
        public DbSet<Platforms> platforms { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }

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

            modelBuilder.Entity<Station>()
                .HasOne(s => s.Facilities)
                .WithOne(f => f.Station)
                .HasForeignKey<Facilities>(f => f.Id_Station);

            modelBuilder.Entity<Station>()
                .HasMany(p => p.Platforms)
                .WithOne()
                .HasForeignKey(p => p.Id_Station)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);

            // Configure FeedbackModel
            modelBuilder.Entity<FeedbackModel>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FeedbackModel>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedbackModel>()
                .HasOne<Station>()
                .WithMany()
                .HasForeignKey(f => f.StationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}