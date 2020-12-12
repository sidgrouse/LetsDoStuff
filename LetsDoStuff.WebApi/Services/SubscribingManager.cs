using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class SubscribingManager : ISubscribingService
    {
        private readonly LdsContext db;

        public SubscribingManager(LdsContext context)
        {
            db = context;
        }

        public void SubscribeUserToActivityByNames(string loginUser, string nameActivity)
        {
            User subscriber = db.Users.Where(h => h.Email == loginUser)
                                    .Include(u => u.Partis)
                                    .First();

            Activity activityForSubscribing = db.Activities
                                        .AsNoTracking()
                                        .Where(a => a.Name == nameActivity)
                                        .FirstOrDefault()
                                        ?? throw new ArgumentException($"Activity with Name {nameActivity} has not been found");

            foreach (Activity act in subscriber.Partis)
            {
                if (activityForSubscribing.Id == act.Id)
                {
                    throw new ArgumentException($"{subscriber.Email} has already subscribed to the event {nameActivity}");
                }    
            }

            subscriber.Partis.Add(activityForSubscribing);

            db.SaveChanges();
        }

        public List<ActivityResponse> GetAllActivitiesOfTheUser(string userName)
        {
            var response = db.Users.AsNoTracking()
               .Where(u => u.Email == userName)
               .Include(u => u.Partis)
               .ThenInclude(a => a.Creator)
               .Include(u => u.Partis)
               .ThenInclude(a => a.Tags)
               .FirstOrDefault()
               .Partis
               .Select(a => new ActivityResponse()
               {
                   Id = a.Id,
                   Name = a.Name,
                   Description = a.Description,
                   Capacity = a.Capacity,
                   Creator = new ActivityCreatorResponse()
                   {
                       Id = a.Creator.Id,
                       Name = a.Creator.FirstName,
                       Login = a.Creator.Email
                   },
                   Tags = a.Tags.Select(t => t.Name).ToList()
               });

            return response.ToList();
        }
    }
}
