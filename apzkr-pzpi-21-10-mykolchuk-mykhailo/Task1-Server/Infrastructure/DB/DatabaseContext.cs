using Microsoft.EntityFrameworkCore;
using APZ_backend.Models;

namespace APZ_backend.DB
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Subordinate> Subordinates => Set<Subordinate>();
        public DbSet<Sensor> Sensors => Set<Sensor>();
        public DbSet<Record> Records => Set<Record>();
        public DbSet<EngineSpeedMoment> EngineSpeeds => Set<EngineSpeedMoment>();
        public DbSet<BrakingMoment> SuddenBraking => Set<BrakingMoment>();
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.RegDate)
                .HasDefaultValueSql("FORMAT(GETDATE(), 'dd.MM.yyyy HH:mm')");

            modelBuilder.Entity<User>()
                .Property(c => c.Role)
                .HasDefaultValue("User");

            modelBuilder.Entity<User>()
               .Property(c => c.Language)
               .HasDefaultValue("en");

            modelBuilder.Entity<Subordinate>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Subordinate>()
                .Property(s => s.RegDate)
                .HasDefaultValueSql("FORMAT(GETDATE(), 'dd.MM.yyyy HH:mm')");

            modelBuilder.Entity<Subordinate>()
               .Property(s => s.Language)
               .HasDefaultValue("en");

            modelBuilder.Entity<Sensor>()
               .HasOne(sensor => sensor.Car)
               .WithOne(car => car.Sensor)
               .HasForeignKey<Sensor>(sensor => sensor.CarId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subordinate>()
                .HasOne(sub => sub.Car)
                .WithMany(car => car.Drivers)
                .HasForeignKey(sub => sub.CarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subordinate>()
                .HasOne(sub => sub.Chief)
                .WithMany(user => user.Subordinates)
                .HasForeignKey(sub => sub.ChiefId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Car>()
                .Property(car => car.Added)
                .HasDefaultValueSql("FORMAT(GETDATE(), 'dd.MM.yyyy HH:mm')");

        }
    }
}
