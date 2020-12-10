using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
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
            User subscriber = db.Users.Where(h => h.Login == loginUser)
                                    .Include(u => u.ActivitiesForParticipation)
                                    .First();

            Activity activityForSubscribing = db.Activities
                                        .AsNoTracking()
                                        .Where(a => a.Name == nameActivity)
                                        .FirstOrDefault()
                                        ?? throw new ArgumentException($"Activity with Name {nameActivity} has not been found");

            foreach (Activity act in subscriber.ActivitiesForParticipation)
            {
                if (activityForSubscribing.Id == act.Id)
                {
                    throw new ArgumentException($"{subscriber.Login} has already subscribed to the event {nameActivity}");
                }    
            }

            subscriber.ActivitiesForParticipation.Add(activityForSubscribing);

            db.SaveChanges();
        }

        public List<ActivityResponse> GetAllActivitiesOfTheUser(string userName)
        {
            var response = db.Users.AsNoTracking()
               .Where(u => u.Login == userName)
               .Include(u => u.ActivitiesForParticipation)
               .ThenInclude(a => a.Creator)
               .Include(u => u.ActivitiesForParticipation)
               .ThenInclude(a => a.Tags)
               .FirstOrDefault()
               .ActivitiesForParticipation
               .Select(a => new ActivityResponse()
               {
                   Id = a.Id,
                   Name = a.Name,
                   Description = a.Description,
                   Capacity = a.Capacity,
                   Creator = new ActivityCreatorResponse()
                   {
                       Id = a.Creator.Id,
                       Name = a.Creator.Name,
                       Login = a.Creator.Login
                   },
                   Tags = a.Tags.Select(t => t.Name).ToList()
               });

            return response.ToList();
        }
    }
}
