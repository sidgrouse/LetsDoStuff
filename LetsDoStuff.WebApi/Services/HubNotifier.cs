using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public class HubNotifier : IHubNotifier
    {
        private readonly IHubContext<ParticipationHub> _hubContext;
        private readonly LdsContext db;

        public HubNotifier(IHubContext<ParticipationHub> hubContext, LdsContext DBcontext)
        {
            _hubContext = hubContext;
            db = DBcontext;
        }

        public void NotifyAboutInvitations(int creatorId, int activityId, int userId)
        {
            throw new NotImplementedException();
        }

        public void NotifyAboutNewParticipationRequest(int activityId, int userId)
        {
            var activity = db.Activities.AsNoTracking()
                .Include(a => a.Creator)
                .FirstOrDefault(u => u.Id == activityId)
                ?? throw new ArgumentException("Woop, something went wrong. We are so sorry about this.");

            var user = db.Users.AsNoTracking()
                .FirstOrDefault(u => u.Id == userId)
                ?? throw new ArgumentException("Woop, something went wrong. We are so sorry about this.");

            _hubContext.Clients.User(activity.CreatorId.ToString()).SendAsync("NotifyAboutParticipation", $"{activity.Creator.FirstName}",  $"{user.FirstName + " " + user.LastName} wants to take a part in your '{activity.Name}' Activity");
        }

        public void NotifyAboutOwnerActivitiesAnswering(int activityId, int userId, bool isAccepted)
        {
            var user = db.Users.AsNoTracking()
                .FirstOrDefault(u => u.Id == userId)
                ?? throw new ArgumentException("Woop, something went wrong. We are so sorry about this.");

            var activity = db.Activities.AsNoTracking()
                .FirstOrDefault(u => u.Id == activityId)
                ?? throw new ArgumentException("Woop, something went wrong. We are so sorry about this.");

            if (isAccepted)
            {
                _hubContext.Clients.User(userId.ToString()).SendAsync("NotifyAboutAcception", $"{user.FirstName}", $"You was accepted like a participant of the {activity.Name} Activity");
            }
            else
            {
                _hubContext.Clients.User(userId.ToString()).SendAsync("NotifyAboutRejection", $"{user.FirstName}", $"You was rejected like a participant of the {activity.Name} Activity");
            }
        }
    }
}
