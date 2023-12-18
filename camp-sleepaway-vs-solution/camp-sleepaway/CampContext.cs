using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace camp_sleepaway
{
    public class CampContext : DbContext
    {
        public DbSet<Camper> Campers { get; set; }
        public DbSet<Counselor> Counselors { get; set;}
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

            // Connect to database
            optionsBuilder.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine,
                new[] { DbLoggerCategory.Database.Name },
                LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}
