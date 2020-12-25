using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationService
    {
        void AddParticipation(int userId, int activityId);

        void RemoveParticipation(int userId, int activityId);

        List<ParticipationResponse> GetParticipationInfo(int userId);

        void AcceptParticipant(int creatorId, int acticityId, int participantId);

        void RejectParticipant(int creatorId, int acticityId, int participantId);

        List<ParticipantResponse> GetParticipantInfo(int creatorId);
    }
}
