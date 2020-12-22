using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using Microsoft.AspNetCore.SignalR;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public class HubNotifier : IHubNotifier
    {
        private readonly IHubContext<SampleHub> _hubContext;
        private readonly LdsContext db;

        public HubNotifier(IHubContext<SampleHub> hubContext, LdsContext DBcontext)
        {
            _hubContext = hubContext;
            db = DBcontext;
        }

        public void NotifyAboutInvitations(int creatorId, int activityId, int userId)
        {
            _hubContext.Clients.User(userId.ToString()).SendAsync("Notify", $"Invitation");
            throw new NotImplementedException();
        }

        public void NotifyAboutNewParticipationRequest(int activiyId, int userId)
        {
            var creatorId = db.Activities.Find(activiyId).CreatorId;
            _hubContext.Clients.User(creatorId.ToString()).SendAsync("Notify", $"Someone with id {userId} want to take a part in your Activity");
        }

        public void NotifyAboutOwnerActivitiesAnswering(int activitId, int userId, bool isAccepted)
        {
            if (isAccepted)
            {
                _hubContext.Clients.User(userId.ToString()).SendAsync("Notify", $"User's ticker with id {userId} was accepted by a Creator");
            }
            else
            {
                _hubContext.Clients.User(userId.ToString()).SendAsync("Notify", $"User's ticker with id {userId} was rejected by a Creator");
            }
        }
    }
}
