using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi
{
    [AllowAnonymous]
    public static class Program
    {
        public static void Main()
        {
            var settingsFileName = "appsettings.json";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var baseUrl = new ConfigurationBuilder().AddJsonFile(settingsFileName).Build().GetValue<string>("BaseUrl");

            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration((host, configurationBuilder) =>
                {
                  configurationBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((context, logging) =>
                {
                    var env = context.HostingEnvironment;
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.AddConsole();
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Critical)
                    .AddFilter("Default", LogLevel.Trace)
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Information)
                    .AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Warning);
                })
                .UseStartup<Startup>()
                .UseUrls(baseUrl)
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<LdsContext>();
                context.Database.EnsureDeleted();
                SeedTestData(context);
            }

            host.Run();
        }

        private static void SeedTestData(LdsContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                context.Add(new User("user1", "Roman", "Onofreichuk", "test@gmail.com", "123456789test", "Admin") { });
                context.Add(new User("user2", "Dee", "Snider", "snider@gmail.com", "1234test56789test", "Admin") { });
                context.Add(new User("user3", "Alice", "Cooper", "acooper@gmail.com", "test12334556", "Admin") { });
                context.SaveChanges();
            }
        }
    }
}