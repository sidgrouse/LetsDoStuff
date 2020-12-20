using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;

namespace LetsDoStuff.WebApi.Services
{
    public class AcceptionService : IAcceptionService
    {
        public void Accept(int participanteId)
        {
            throw new NotImplementedException();
        }

        public List<UserResponse> GetAcrivityParticipantes(int ActicitiId)
        {
            throw new NotImplementedException();
        }

        public List<UserResponse> GetAllParticipantes()
        {
            throw new NotImplementedException();
        }

        public void Reject(int participanteId)
        {
            throw new NotImplementedException();
        }
    }
}
