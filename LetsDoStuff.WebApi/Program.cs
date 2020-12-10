using System;
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
                var userService = services.GetService<IUserService>();
                SeedTestData(context, userService);
            }

            host.Run();
        }

        private static void SeedTestData(LdsContext context, IUserService userService)
        {
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                userService.RegisterUser(new RegisterRequest()
                {
                    FirstName = "Roman",
                    LastName = "Onofreichuk",
                    Email = "test@gmail.com",
                    Password = "12test",
                    ConfirmPassword = "12test"
                });
                userService.RegisterUser(new RegisterRequest()
                {
                    FirstName = "Dee",
                    LastName = "Snider",
                    Email = "snider@gmail.com",
                    Password = "89test",
                    ConfirmPassword = "89test"
                });
                userService.RegisterUser(new RegisterRequest()
                {
                    FirstName = "Alice",
                    LastName = "Cooper",
                    Email = "acooper@gmail.com",
                    Password = "test123",
                    ConfirmPassword = "test123"
                });
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

            var userDee = context.Users.FirstOrDefault(u => u.UserName == "user2");
            var userAlice = context.Users.FirstOrDefault(u => u.UserName == "user3");
            var tagMusic = context.Tags.FirstOrDefault(itm => itm.Name == "Music");
            var tagIntellectual = context.Tags.FirstOrDefault(itm => itm.Name == "Intellectual");
            var tagOpenAir = context.Tags.FirstOrDefault(itm => itm.Name == "Intellectual");

            if (!context.Activities.Any())
            {
                context.Activities.Add(new Activity() { Creator = userDee, Name = "Octoberfest", Description = "Go for beer n music in my car", Capacity = 4, Tags = new List<Tag> { tagMusic, tagOpenAir } });
                context.Activities.Add(new Activity() { Creator = userAlice, Name = "Home violin concert", Description = "I am gonna play old good classic songs in my place", Capacity = 20, Tags = new List<Tag> { tagMusic } });
                context.Activities.Add(new Activity() { Creator = userDee, Name = "Hicking in Altai Mountains", Description = "Weekend trip", Capacity = 4 });
                context.Activities.Add(new Activity() { Creator = userDee, Name = "PubQuiz mindstorm", Description = "In central perk", Capacity = 6, Tags = new List<Tag> { tagIntellectual } });
                context.SaveChanges();
            }
        }
    }
}