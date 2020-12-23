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
    public class ParticipantAcceptorService : IParticipantAcceptorService
    {
        private readonly LdsContext db;

        public ParticipantAcceptorService(LdsContext context)
        {
            db = context;
        }

        public void AcceptParticipant(int creatorId, int acticitiId, int participantId)
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

        public void RejectParticipant(int creatorId, int acticitiId, int participanteId)
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

        public List<ParticipationResponseForCreator> GetParticipationsInfo(int creatorId)
        {
            var activities = db.Activities.AsNoTracking()
                .Include(a => a.ParticipantsTickets)
                .ThenInclude(pc => pc.User)
                .Where(a => a.CreatorId == creatorId).ToList();

            List<ParticipationResponseForCreator> info = new List<ParticipationResponseForCreator>();

            foreach (var act in activities)
            {
                info.AddRange(act.ParticipantsTickets.Select(pc => new ParticipationResponseForCreator()
                {
                    ActivityName = act.Name,
                    ActicityId = act.Id,
                    ContactName = pc.User.FirstName + " " + pc.User.LastName,
                    ContactId = pc.User.Id,
                    Email = pc.User.Email,
                    ProfileLink = pc.User.ProfileLink,
                    AcceptAsParticipant = pc.IsParticipant
                }));
            }

            return info;
        }
    }
}
