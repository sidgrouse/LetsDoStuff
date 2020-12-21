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
            var creatorActivity = db.Activities.Include(a => a.ParticipantСertificates)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (creatorActivity.CreatorId == idCreator)
            {
                var pc = creatorActivity.ParticipantСertificates.FirstOrDefault(pc => pc.UserId == participanteId)
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
            var creatorActivity = db.Activities.Include(a => a.ParticipantСertificates)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (creatorActivity.CreatorId == idCreator)
            {
                var pc = creatorActivity.ParticipantСertificates.FirstOrDefault(pc => pc.UserId == participanteId)
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

        public List<MayParticipationResponse> GetAcrivityMayParticipantes(int idCreator, int acticitiId)
        {
            var creatorActivity = db.Activities.AsNoTracking()
                .Include(a => a.ParticipantСertificates).ThenInclude(pc => pc.User)
                .Include(a => a.Creator)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (creatorActivity.Creator.Id == idCreator)
            {
                var mayParticipants = creatorActivity.ParticipantСertificates.Where(pc => pc.IsParticipante == false).Select(pc => pc.User).ToList();

                return mayParticipants.Select(u => new MayParticipationResponse()
                {
                    ContactName = u.FirstName + u.FirstName,
                    Email = u.Email,
                    ProfileLink = u.ProfileLink,
                    ActivityName = creatorActivity.Name,
                    ActicityId = creatorActivity.Id
                }).ToList();
            }

            throw new ArgumentException($"User with id {idCreator} is not the creator of the activity {acticitiId}!");
        }

        public List<MayParticipationResponse> GetAllMayParticipantes(int idCreator)
        {
            throw new NotImplementedException();
        }
    }
}
