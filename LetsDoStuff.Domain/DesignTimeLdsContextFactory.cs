using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LetsDoStuff.Domain
{
    public class DesignTimeLdsContextFactory : IDesignTimeDbContextFactory<LdsContext>
    {
        public LdsContext CreateDbContext(string[] args)
        {
            var settingsFileName = "appsettings.json";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var connectionString = new ConfigurationBuilder()
                .SetBasePath(basePath) // Set the path of the current directory.
                .AddJsonFile(settingsFileName) // Get the configuration from the "appsettings.json".
                .Build() // Build the configuration.
                .GetConnectionString("DefaultConnection"); // Get the database connection string from configuration.
            
            var optionsBuilder = new DbContextOptionsBuilder<LdsContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return new LdsContext(options);
        }
    }
}
