using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class AcceptionService : IAcceptionService
    {
        private readonly LdsContext db;

        public AcceptionService(LdsContext context)
        {
            db = context;
        }

        public void Accept(int idCreator, int acticitiId, int participanteId)
        {
            var activityCreator = db.Activities.Include(a => a.ParticipantСertificates)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activityCreator.CreatorId == idCreator)
            {
                var pc = activityCreator.ParticipantСertificates.FirstOrDefault(pc => pc.UserId == participanteId)
                    ?? throw new ArgumentException($"User with id {participanteId} has not been found like someone who's willing take a part in the Activity with id {acticitiId}");

                if (!pc.IsParticipante)
                {
                    pc.IsParticipante = true;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    throw new ArgumentException($"User with id {participanteId} has already been marked as participante");
                }
            }
            else
            {
                throw new ArgumentException($"User with id {idCreator} is not the creator of the activity {acticitiId}!");
            }
        }

        public void Reject(int idCreator, int acticitiId, int participanteId)
        {
            var activityCreator = db.Activities.Include(a => a.ParticipantСertificates)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activityCreator.CreatorId == idCreator)
            {
                var pc = activityCreator.ParticipantСertificates.FirstOrDefault(pc => pc.UserId == participanteId)
                    ?? throw new ArgumentException($"User with id {participanteId} has not been found like someone who's willing take a part in the Activity with id {acticitiId}");

                if (pc.IsParticipante)
                {
                    pc.IsParticipante = false;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    throw new ArgumentException($"User with id {participanteId} has already been marked as participante");
                }
            }
            else
            {
                throw new ArgumentException($"User with id {idCreator} is not the creator of the activity {acticitiId}!");
            }
        }

        public List<ParticipationResponseForCreator> GetAllParticipantes(int idCreator)
        {
            throw new NotImplementedException();
        }
    }
}
