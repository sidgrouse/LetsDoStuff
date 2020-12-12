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

        public void MakeUserSubscribedToActivityByEmailAndId(string emailUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Email == emailUser)
                                    .Include(u => u.Partis)
                                    .First();

            Activity activityForSubscribing = db.Activities
                                        .Find(idActivity)
                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            foreach (Activity act in subscriber.Partis)
            {
                if (activityForSubscribing.Id == act.Id)
                {
                    throw new ArgumentException($"{subscriber.Email} has already subscribed to the event with id {idActivity}");
                }    
            }

            subscriber.Partis.Add(activityForSubscribing);

            db.SaveChanges();
        }

        public void MakeUserUnsubscribedToActivityByEmailAndId(string emailUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Email == emailUser)
                                    .Include(u => u.Partis)
                                    .First();

            Activity activityForUnsubscribing = db.Activities
                                        .Find(idActivity)
                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            foreach (Activity act in subscriber.Partis)
            {
                if (activityForUnsubscribing.Id == act.Id)
                {
                    subscriber.Partis.Remove(activityForUnsubscribing);
                    db.SaveChanges();
                    return;
                }
            }

            throw new ArgumentException($"{subscriber.Email} hasn't had subscription to the event with id {idActivity}");
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
