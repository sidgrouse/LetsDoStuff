﻿using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipantAcceptorService
    {
        void AcceptParticipant(int creatorId, int acticitiId, int participanteId);

        void RejectParticipant(int creatorId, int acticitiId, int participanteId);

        List<ParticipantResponse> GetParticipantInfo(int creatorId);
    }
}