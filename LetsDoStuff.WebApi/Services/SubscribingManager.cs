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

        public void SubscribeUserToActivityByNames(string nameUser, string nameActivity)
        {
            User subscriber = FindUserByName(nameUser);
            Activity activityForSubscribing = FindActivityByName(nameActivity);
            foreach (Activity act in subscriber.ActivitiesForAttending)
            {
                if (activityForSubscribing == act)
                {
                    throw new ArgumentException($"{subscriber} has already subscribed to the event {nameActivity}");
                }    
            }

            subscriber.ActivitiesForAttending.Add(activityForSubscribing);

            db.SaveChanges();
        }

        public List<ActivityResponse> GetAllActivitiesOfTheUser(string userName)
        {
            var response = db.Users.AsNoTracking()
               .Where(u => u.Login == userName)
               .Include(u => u.ActivitiesForAttending)
               .ThenInclude(a => a.Creator)
               .FirstOrDefault()
               .ActivitiesForAttending
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

        private User FindUserByName(string nameUser) => db.Users.AsNoTracking().Where(h => h.Login == nameUser).Include(u => u.ActivitiesForAttending).FirstOrDefault();

        private Activity FindActivityByName(string nameActivity) => db.Activities.Where(a => a.Name == nameActivity).First() ?? throw new ArgumentException($"Activity with Name {nameActivity} has not been found");
    }
}
