using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationRequesterService
    {
        void AddParticipation(int idUser, int idActivity);

        void RemoveParticipation(int idUser, int idActivity);

        List<ParticipationResponseForUser> GetUsersParticipations(int userId);
    }
}
