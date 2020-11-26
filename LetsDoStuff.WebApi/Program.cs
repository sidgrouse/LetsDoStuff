using System.IO;
using System.Linq;
using System.Reflection;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LetsDoStuff.WebApi
{
    public static class Program
    {
        public static void Main()
        {
            var settingsFileName = "appsettings.json";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var baseUrl = new ConfigurationBuilder().AddJsonFile(settingsFileName).Build().GetValue<string>("BaseUrl");

            var connectionString = new ConfigurationBuilder()
                .SetBasePath(basePath) // Set the path of the current directory.
                .AddJsonFile(settingsFileName) // Get the configuration from the "appsettings.json".
                .Build() // Build the configuration.
                .GetConnectionString("DefaultConnection"); // Get the database connection string from configuration.

            var optionsBuilder = new DbContextOptionsBuilder<LdsContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            SeedTestData(options);

            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration(
                    (host, configurationBuilder) =>
                    {
                        configurationBuilder.AddJsonFile("appsettings.json");
                    })
                .UseStartup<Startup>()
                .UseUrls(baseUrl)
                .Build();

            host.Run();
        }

        private static void SeedTestData(DbContextOptions<LdsContext> options)
        {
            using (LdsContext context = new LdsContext(options))
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User { Name = "Tom", Login = "Tom1", Passeword = "1234", Age = 33, Role = "admin" });
                    context.Users.Add(new User { Name = "Alice", Login = "Alice1", Passeword = "0000", Age = 26, Role = "user" });
                    context.SaveChanges();
                }
            }
        }
    }
}