using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationRequestorService
    {
        void AddParticipation(int userId, int activityId);

        void RemoveParticipation(int userId, int activityId);

        List<ParticipationResponse> GetParticipationInfo(int userId);
    }
}
