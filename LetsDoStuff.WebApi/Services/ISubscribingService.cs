using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.Domain.Models;

namespace LetsDoStuff.WebApi.Services
{
    public interface ISubscribingService
    {
        void SubscribeUserToActivityByNames(string nameUser, string nameActivity);

        List<Activity> GetAllActivitiesOfTheUser(string userName);
    }
}
