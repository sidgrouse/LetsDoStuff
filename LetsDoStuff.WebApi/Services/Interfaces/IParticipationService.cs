﻿using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IParticipationService
    {
        void AddParticipation(int idUser, int idActivity);

        void RemoveParticipation(int idUser, int idActivity);

        List<ActivityResponse> GetUsersParticipations(int userId);
    }
}