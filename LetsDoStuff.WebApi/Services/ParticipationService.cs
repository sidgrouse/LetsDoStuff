using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class ParticipationService : IParticipationService
    {
        private readonly LdsContext db;

        public ParticipationService(LdsContext context)
        {
            db = context;
        }

        public void AddParticipation(int userId, int activityId)
        {
            User user = db.Users.Where(h => h.Id == userId)
                .Include(u => u.ParticipationActivities)
                .FirstOrDefault()
                 ?? throw new ArgumentException($"User with id {userId} has not been found");

            Activity activity = db.Activities
                .Find(activityId)
                ?? throw new ArgumentException($"Activity with id {activityId} has not been found");

            if (user.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{user.Email} has already participated in the event with id {activity.Id}");
            }

            user.ParticipationActivities.Add(activity);

            db.SaveChanges();
        }

        public void RemoveParticipation(int userId, int activityId)
        {
            User user = db.Users.Where(h => h.Id == userId)
                .Include(u => u.ParticipationActivities)
                .FirstOrDefault()
                ?? throw new ArgumentException($"User with id {userId} has not been found");

            Activity activity = db.Activities
                .Find(activityId)
                ?? throw new ArgumentException($"Activity with id {activityId} has not been found");

            if (!user.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{user.Email} hasn't participated in the event with id {activity.Id}");
            }

            user.ParticipationActivities.Remove(activity);
            db.SaveChanges();
        }

        public List<ParticipationResponse> GetParticipationInfo(int userId)
        {
            var response = db.Users.AsNoTracking()
               .Where(u => u.Id == userId)
               .Include(u => u.ParticipationActivities)
               .ThenInclude(a => a.Creator)
               .Include(u => u.ParticipationActivities)
               .ThenInclude(a => a.Tags)
               .Include(a => a.ParticipationActivities)
               .ThenInclude(pa => pa.ParticipantsTickets)
               .FirstOrDefault()
               .ParticipationActivities
               .Select(a => new ParticipationResponse()
               {
                   Id = a.Id,
                   Name = a.Name,
                   Description = a.Description,
                   Capacity = a.Capacity,
                   AcceptAsParticipant = a.ParticipantsTickets.FirstOrDefault(pc => pc.UserId == userId).IsParticipant,
                   Creator = new ActivityCreatorResponse()
                   {
                       Name = a.Creator.FirstName,
                       ProfileLink = a.Creator.ProfileLink
                   },
                   Tags = a.Tags.Select(t => t.Name).ToList()
               });

            return response.ToList();
        }

        public void AcceptParticipant(int creatorId, int acticityId, int participantId)
        {
            var activity = db.Activities.Include(a => a.ParticipantsTickets)
                .FirstOrDefault(a => a.Id == acticityId)
                ?? throw new ArgumentException($"Activity with id {acticityId} has not been found");

            if (activity.CreatorId == creatorId)
            {
                var participantTicket = activity.ParticipantsTickets.FirstOrDefault(pc => pc.UserId == participantId)
                    ?? throw new ArgumentException($"User with id {participantId} has not been found like someone who's willing take a part in the Activity with id {acticityId}");

                if (!participantTicket.IsParticipant)
                {
                    participantTicket.IsParticipant = true;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    throw new ArgumentException($"User with id {participantId} has already been marked as participant");
                }
            }
            else
            {
                throw new ArgumentException($"Sorry, but you are not a creator of the activity with id {creatorId}!");
            }
        }

        public void RejectParticipant(int creatorId, int acticityId, int participantId)
        {
            var activity = db.Activities.Include(a => a.Participants)
                .FirstOrDefault(a => a.Id == acticityId)
                ?? throw new ArgumentException($"Activity with id {acticityId} has not been found");

            if (activity.CreatorId == creatorId)
            {
                var rejectingUser = activity.Participants.FirstOrDefault(u => u.Id == participantId)
                    ?? throw new ArgumentException($"User with id {participantId} has not been found");

                activity.Participants.Remove(rejectingUser);
                db.SaveChanges();
                return;
            }
            else
            {
                throw new ArgumentException($"Sorry, but you are not a creator of the activity with id {creatorId}!");
            }
        }

        public List<ParticipantResponse> GetParticipantInfo(int creatorId)
        {
            var activities = db.Activities.AsNoTracking()
                .Include(a => a.ParticipantsTickets)
                .ThenInclude(pc => pc.User)
                .Where(a => a.CreatorId == creatorId).ToList();

            List<ParticipantResponse> info = new List<ParticipantResponse>();

            foreach (var act in activities)
            {
                info.AddRange(act.ParticipantsTickets.Select(pc => new ParticipantResponse()
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
