using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface ISubscribingService
    {
        void SubscribeUserToActivityByNames(string nameUser, string nameActivity);

        List<ActivityResponse> GetAllActivitiesOfTheUser(string userName);
    }
}
