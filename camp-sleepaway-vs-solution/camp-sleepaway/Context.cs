using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace camp_sleepaway
{
    public class Context : DbContext
    {
        public DbSet<Camper> Campers { get; set; }
        public DbSet<Counselor> Counselors { get; set;}
        public DbSet<NextOfKin> NextOfKins { get; set; }
        public DbSet<Cabin> Cabins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            // Read options from .json-file

            var connectionString = configuration.GetConnectionString("local");
            // Build connection string

            optionsBuilder.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine,
                new[] { DbLoggerCategory.Database.Name },
                LogLevel.Information)
                .EnableSensitiveDataLogging();
            // Connect to database
        }
    }
}
