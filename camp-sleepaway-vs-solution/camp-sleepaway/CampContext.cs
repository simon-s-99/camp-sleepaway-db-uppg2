using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public class CampContext : DbContext
    {
        public DbSet<Camper> Campers { get; set; }
        public DbSet<Counselor> Counselors { get; set; }
        public DbSet<NextOfKin> NextOfKins { get; set; }
        public DbSet<Cabin> Cabins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Read options from .json-file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Build connection string
            var connectionString = configuration.GetConnectionString("local");

            // Connect to database | code lines related to logging should be commented out/removed in final version
            optionsBuilder.UseSqlServer(connectionString);
            //.LogTo(Console.WriteLine,
            //new[] { DbLoggerCategory.Database.Name },
            //LogLevel.Information)
            //.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Counselor>()
                .HasOne<Cabin>(counselor => counselor.Cabin)
                .WithOne(cabin => cabin.Counselor)
                .HasForeignKey<Cabin>(cabin => cabin.CounselorId);

            modelBuilder.Entity<Camper>()
                .HasOne<Cabin>(camper => camper.Cabin)
                .WithMany(cabin => cabin.Campers)
                .HasForeignKey(camper => camper.CabinId);

            modelBuilder.Entity<NextOfKin>()
                .HasOne<Camper>(nextOfKin => nextOfKin.Camper)
                .WithMany(camper => camper.NextOfKins)
                .HasForeignKey(nextOfKin => nextOfKin.CamperId);

            base.OnModelCreating(modelBuilder);
        }
        // Using Fluent API, we create two one-to-many relations and one one-to-one relation

    }
}
