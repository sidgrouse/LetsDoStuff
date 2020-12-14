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
    public class SubscribingService : ISubscribingService
    {
        private readonly LdsContext db;
        
        public SubscribingService(LdsContext context)
        {
            db = context;
        }

        public void MakeUserSubscribedToActivityByEmailAndId(string emailUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Email == emailUser)
                                   .Include(u => u.ParticipationActivities)
                                   .Single();

            Activity participation = db.Activities
                                                .Find(idActivity)
                                                 ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            SubscribeCreator(subscriber, participation);
        }

        public void MakeUserUnsubscribedToActivityByEmailAndId(string emailUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Email == emailUser)
                                   .Include(u => u.ParticipationActivities)
                                   .Single();

            Activity participation = db.Activities
                                        .Find(idActivity)
                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            UnsubscribeCreator(subscriber, participation);
        }

        public void MakeUserSubscribedToActivityByIds(int idUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Id == idUser)
                                  .Include(u => u.ParticipationActivities)
                                  .FirstOrDefault()
                                  ?? throw new ArgumentException($"User with id {idUser} has not been found");

            Activity participation = db.Activities
                                                .Find(idActivity)
                                                 ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            SubscribeCreator(subscriber, participation);
        }

        public void MakeUserUnsubscribedToActivityByIds(int idUser, int idActivity)
        {
            User subscriber = db.Users.Where(h => h.Id == idUser)
                                 .Include(u => u.ParticipationActivities)
                                 .FirstOrDefault()
                                 ?? throw new ArgumentException($"User with id {idUser} has not been found");

            Activity participation = db.Activities
                                        .Find(idActivity)
                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            UnsubscribeCreator(subscriber, participation);
        }

        public List<ActivityResponse> GetAllActivitiesOfTheUser(string userName)
        {
            var response = db.Users.AsNoTracking()
               .Where(u => u.Email == userName)
               .Include(u => u.ParticipationActivities)
               .ThenInclude(a => a.Creator)
               .Include(u => u.ParticipationActivities)
               .ThenInclude(a => a.Tags)
               .FirstOrDefault()
               .ParticipationActivities
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

        #region

        private void SubscribeCreator(User subscriber, Activity activity)
        {
            if (subscriber.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{subscriber.Email} has already subscribed to the event with id {activity.Id}");
            }

            subscriber.ParticipationActivities.Add(activity);

            db.SaveChanges();
        }

        private void UnsubscribeCreator(User subscriber, Activity activity)
        {
            if (!subscriber.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{subscriber.Email} hasn't had subscription to the event with id {activity.Id}");
            }

            subscriber.ParticipationActivities.Remove(activity);
            db.SaveChanges();
        }
        #endregion
    }
}
