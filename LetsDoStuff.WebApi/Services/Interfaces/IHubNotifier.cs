using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IHubNotifier
    {
        void NotifyAboutNewParticipationRequest(int activityId, int userId);

        void NotifyAboutOwnerActivitiesAnswering(int activityId, int userId, bool isAccepted);

        void NotifyAboutInvitations(int creatorId, int activityId, int userId);
    }
}
