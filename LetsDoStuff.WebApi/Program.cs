using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi
{
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
                SeedTestData(context);
            }

            host.Run();
        }

        private static void SeedTestData(LdsContext context)
        {
            int userID = 1;
            string userRoleName = "User";
            string adminRoleName = "Admin";
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                context.Users.Add(new User(profileLink: $"user{userID++}", firstName: "Dee", lastName: "Snider", email: "dee@gmail.com", password: "12test", userRoleName));
                context.Users.Add(new User(profileLink: $"user{userID++}", firstName: "Alice", lastName: "Cooper", email: "alice@gmail.com", password: "test12", userRoleName));
                context.Users.Add(new User(profileLink: $"user{userID++}", firstName: "Roman", lastName: "Onofreichuk", email: "admin@gmail.com", password: "123", adminRoleName));
                context.SaveChanges();
            }

            if (!context.Tags.Any())
            {
                context.Tags.Add(new Tag { Name = "Music" });
                context.Tags.Add(new Tag { Name = "Open-air" });
                context.Tags.Add(new Tag { Name = "Indoor" });
                context.Tags.Add(new Tag { Name = "Sport" });
                context.Tags.Add(new Tag { Name = "Intellectual" });
                context.SaveChanges();
            }

            var userDee = context.Users.FirstOrDefault(u => u.ProfileLink == "user1");
            var userAlice = context.Users.FirstOrDefault(u => u.ProfileLink == "user2");
            var tagMusic = context.Tags.FirstOrDefault(itm => itm.Name == "Music");
            var tagIntellectual = context.Tags.FirstOrDefault(itm => itm.Name == "Intellectual");
            var tagOpenAir = context.Tags.FirstOrDefault(itm => itm.Name == "Intellectual");

            if (!context.Activities.Any())
            {
                context.Activities.Add(new Activity() { Creator = userDee, DateStart = DateTime.Parse("2021-09-18"), Name = "Octoberfest", Description = "Go for beer n music in my car", Capacity = 4, Tags = new List<Tag> { tagMusic, tagOpenAir } });
                context.Activities.Add(new Activity() { Creator = userAlice, DateStart = DateTime.Parse("2021-01-01"), Name = "Home violin concert", Description = "I am gonna play old good classic songs in my place", Capacity = 20, Tags = new List<Tag> { tagMusic } });
                context.Activities.Add(new Activity() { Creator = userDee, DateStart = DateTime.Parse("2020-12-29"), Name = "Hicking in Altai Mountains", Description = "Weekend trip", Capacity = 4 });
                context.Activities.Add(new Activity() { Creator = userDee, DateStart = DateTime.Parse("2020-12-23"), Name = "PubQuiz mindstorm", Description = "In central park", Capacity = 6, Tags = new List<Tag> { tagIntellectual } });
                context.SaveChanges();
            }
        }
    }
}