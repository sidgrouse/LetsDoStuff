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
    public class ParticipationRequestorService : IParticipationRequesterService
    {
        private readonly LdsContext db;
        
        public ParticipationRequestorService(LdsContext context)
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
    }
}
