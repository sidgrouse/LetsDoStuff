using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface ISubscribingService
    {
        void MakeUserSubscribedToActivityByEmailAndId(string emailUser, int idActivity);

        void MakeUserUnsubscribedToActivityByEmailAndId(string emailUser, int idActivity);

        List<ActivityResponse> GetAllActivitiesOfTheUser(string emailUser);
    }
}
