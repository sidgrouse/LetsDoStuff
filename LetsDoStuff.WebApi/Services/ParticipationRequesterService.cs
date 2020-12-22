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
    public class ParticipationRequesterService : IParticipationRequesterService
    {
        private readonly LdsContext db;
        
        public ParticipationRequesterService(LdsContext context)
        {
            db = context;
        }

        public void AddParticipation(int idUser, int idActivity)
        {
            User user = db.Users.Where(h => h.Id == idUser)
                .Include(u => u.ParticipationActivities)
                .FirstOrDefault()
                 ?? throw new ArgumentException($"User with id {idUser} has not been found");

            Activity activity = db.Activities
                .Find(idActivity)
                ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            if (user.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{user.Email} has already participated in the event with id {activity.Id}");
            }

            user.ParticipationActivities.Add(activity);

            db.SaveChanges();
        }

        public void RemoveParticipation(int idUser, int idActivity)
        {
            User user = db.Users.Where(h => h.Id == idUser)
                .Include(u => u.ParticipationActivities)
                .FirstOrDefault()
                ?? throw new ArgumentException($"User with id {idUser} has not been found");

            Activity activity = db.Activities
                .Find(idActivity)
                ?? throw new ArgumentException($"Activity with id {idActivity} has not been found");

            if (!user.ParticipationActivities.Contains(activity))
            {
                throw new ArgumentException($"{user.Email} hasn't participated in the event with id {activity.Id}");
            }

            user.ParticipationActivities.Remove(activity);
            db.SaveChanges();
        }

        public List<ParticipationResponseForUser> GetUsersParticipations(int userId)
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
               .Select(a => new ParticipationResponseForUser()
               {
                   Id = a.Id,
                   Name = a.Name,
                   Description = a.Description,
                   Capacity = a.Capacity,
                   AcceptAsParticipant = a.ParticipantsTickets.FirstOrDefault(pc => pc.UserId == userId).IsParticipante,
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
