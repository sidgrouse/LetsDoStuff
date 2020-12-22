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

        public void AcceptParticipation(int creatorId, int acticitiId, int participantId)
        {
            var activity = db.Activities.Include(a => a.ParticipantsTickets)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activity.CreatorId == creatorId)
            {
                var participantTicket = activity.ParticipantsTickets.FirstOrDefault(pc => pc.UserId == participantId)
                    ?? throw new ArgumentException($"User with id {participantId} has not been found like someone who's willing take a part in the Activity with id {acticitiId}");

                if (!participantTicket.IsParticipant)
                {
                    participantTicket.IsParticipant = true;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    throw new ArgumentException($"User with id {participantId} has already been marked as participante");
                }
            }
            else
            {
                throw new ArgumentException($"Sorry, but you are not a creator of the activity with id {creatorId}!");
            }
        }

        public void RejectParticipation(int creatorId, int acticitiId, int participanteId)
        {
            var activity = db.Activities.Include(a => a.Participants)
                .FirstOrDefault(a => a.Id == acticitiId)
                ?? throw new ArgumentException($"Activity with id {acticitiId} has not been found");

            if (activity.CreatorId == creatorId)
            {
                var rejectingUser = activity.Participants.FirstOrDefault(u => u.Id == participanteId)
                    ?? throw new ArgumentException($"User with id {participanteId} has not been found");

                activity.Participants.Remove(rejectingUser);
                db.SaveChanges();
                return;
            }
            else
            {
                throw new ArgumentException($"Sorry, but you are not a creator of the activity with id {creatorId}!");
            }
        }

        public List<ParticipationResponseForCreator> GetAllParticipations(int creatorId)
        {
            var activitiesOfCreator = db.Activities.AsNoTracking()
                .Include(a => a.ParticipantsTickets)
                .ThenInclude(pc => pc.User)
                .Where(a => a.CreatorId == creatorId).ToList();

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
                    AcceptAsParticipant = pc.IsParticipant
                }));
            }

            return result;
        }
    }
}
