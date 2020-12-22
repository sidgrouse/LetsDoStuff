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
    public class ParticipationAccepterService : IParticipationAccepterService
    {
        private readonly LdsContext db;

        public ParticipationAccepterService(LdsContext context)
        {
            db = context;
        }

        public void Accept(int idCreator, int acticitiId, int participanteId)
        {
            var activityCreator = db.Activities.Include(a => a.ParticipantsTickets)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activityCreator.CreatorId == idCreator)
            {
                var pc = activityCreator.ParticipantsTickets.FirstOrDefault(pc => pc.UserId == participanteId)
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
            var activityCreator = db.Activities.Include(a => a.Participants)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activityCreator.CreatorId == idCreator)
            {
                var rejectingUser = activityCreator.Participants.FirstOrDefault(u => u.Id == participanteId)
                    ?? throw new ArgumentException($"User with id {participanteId} has not been found");

                activityCreator.Participants.Remove(rejectingUser);
                db.SaveChanges();
                return;
            }
            else
            {
                throw new ArgumentException($"User with id {idCreator} is not the creator of the activity {acticitiId}!");
            }
        }

        public List<ParticipationResponseForCreator> GetAllParticipations(int idCreator)
        {
            var activitiesOfCreator = db.Activities.AsNoTracking()
                .Include(a => a.ParticipantsTickets)
                .ThenInclude(pc => pc.User)
                .Where(a => a.CreatorId == idCreator).ToList();

            List<ParticipationResponseForCreator> result = new List<ParticipationResponseForCreator>();

            foreach (var activity in activitiesOfCreator)
            {
                result.AddRange(activity.ParticipantsTickets.Select(pc => new ParticipationResponseForCreator()
                {
                    ActivityName = activity.Name,
                    ActicityId = activity.Id,
                    ContactName = pc.User.FirstName + " " + pc.User.LastName,
                    ContactId = pc.User.Id,
                    Email = pc.User.Email,
                    ProfileLink = pc.User.ProfileLink,
                    AcceptAsParticipant = pc.IsParticipante
                }));
            }

            return result;
        }
    }
}
