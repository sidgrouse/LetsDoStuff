using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;

namespace LetsDoStuff.WebApi.Services
{
    public interface ISubscribingService
    {
        void SubscribeUserToActivityByNames(string nameUser, string nameActivity);

        List<ActivityResponse> GetAllActivitiesOfTheUser(string userName);
    }
}
