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
            User subscriber = FindSubscriberByEmail(emailUser);

            Activity activityForSubscribing = GetActivityOrNull(idActivity)
                                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            if (IsThereActivityinUserPartis(subscriber, activityForSubscribing))
            {
                throw new ArgumentException($"{subscriber.Email} has already subscribed to the event with id {idActivity}");
            }

            subscriber.Partis.Add(activityForSubscribing);

            db.SaveChanges();
        }

        public void MakeUserUnsubscribedToActivityByEmailAndId(string emailUser, int idActivity)
        {
            User subscriber = FindSubscriberByEmail(emailUser);

            Activity activityForUnsubscribing = GetActivityOrNull(idActivity)
                                        ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            if (!IsThereActivityinUserPartis(subscriber, activityForUnsubscribing))
            {
                throw new ArgumentException($"{subscriber.Email} hasn't had subscription to the event with id {idActivity}");
            }

            subscriber.Partis.Remove(activityForUnsubscribing);
            db.SaveChanges();
            return;    
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

        #region
        private User FindSubscriberByEmail(string emailUser) => db.Users.Where(h => h.Email == emailUser)
                                   .Include(u => u.Partis)
                                   .Single();

        private Activity GetActivityOrNull(int idActivity) => db.Activities
                                        .Find(idActivity);

        private bool IsThereActivityinUserPartis(User user, Activity activity)
        {
            foreach (Activity act in user.Partis)
            {
                if (activity.Id == act.Id)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
