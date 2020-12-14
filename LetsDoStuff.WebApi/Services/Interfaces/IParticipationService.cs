using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationService
    {
        void MakeUserSubscribedToActivityByIds(int idUser, int idActivity);

        void MakeUserSubscribedToActivityByEmailAndId(string emailUser, int idActivity);

        void MakeUserUnsubscribedToActivityByIds(int idUser, int idActivity);

        void MakeUserUnsubscribedToActivityByEmailAndId(string emailUser, int idActivity);

        List<ActivityResponse> GetUsersParticipations(int userId);
    }
}
