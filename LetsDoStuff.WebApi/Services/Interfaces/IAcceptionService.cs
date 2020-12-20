using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IAcceptionService
    {
        void Accept(int participanteId);

        void Reject(int participanteId);

        List<UserResponse> GetAcrivityParticipantes(int ActicitiId);

        List<UserResponse> GetAllParticipantes();
    }
}
