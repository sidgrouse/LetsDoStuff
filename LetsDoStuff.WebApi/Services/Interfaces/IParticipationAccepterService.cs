using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationAccepterService
    {
        void Accept(int idCreator, int acticitiId, int participanteId);

        void Reject(int idCreator, int acticitiId, int participanteId);

        List<ParticipationResponseForCreator> GetAllParticipations(int idCreator);
    }
}
