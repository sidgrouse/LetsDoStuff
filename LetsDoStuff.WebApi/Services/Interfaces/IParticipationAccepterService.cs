using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationAccepterService
    {
        void AcceptParticipation(int creatorId, int acticitiId, int participanteId);

        void RejectParticipation(int creatorId, int acticitiId, int participanteId);

        List<ParticipationResponseForCreator> GetAllParticipations(int creatorId);
    }
}
