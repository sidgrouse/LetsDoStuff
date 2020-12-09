using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
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

        public List<Activity> GetAllActivitiesOfTheUser(string userName)
        {
            User currentUser = FindUserByName(userName);
            return currentUser.ActivitiesForAttending.ToList();
        }

        private User FindUserByName(string nameUser) => db.Users.AsNoTracking().Where(h => h.Login == nameUser).Include(u => u.ActivitiesForAttending).FirstOrDefault();

        private Activity FindActivityByName(string nameActivity) => db.Activities.Where(a => a.Name == nameActivity).First() ?? throw new ArgumentException($"Activity with Name {nameActivity} has not been found");
    }
}
