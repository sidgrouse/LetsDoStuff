using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.IO;
using System.Reflection;

namespace LetsDoStuff.WebApi
{
    public static class Program
    {
        static void Main()
        {
            var settingsFileName = "appsettings.json";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var baseUrl = new ConfigurationBuilder().AddJsonFile(settingsFileName).Build().GetValue<string>("BaseUrl");

            SeedTestData();

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

        private static void SeedTestData()
        {
            using (LdsContext context = new LdsContext())
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User { Name = "Tom", Age = 33 });
                    context.Users.Add(new User { Name = "Alice", Age = 26 });
                    context.SaveChanges();
                }
            }
        }
    }
}