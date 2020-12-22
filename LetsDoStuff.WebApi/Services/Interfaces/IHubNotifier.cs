using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IHubNotifier
    {
        void NotifyAboutNewParticipationRequest(int activiyId, int userId);

        void NotifyAboutOwnerActivitiesAnswering(int activitId, int userId, bool isAccepted);

        void NotifyAboutInvitations(int creatorId, int activityId, int userId);
    }
}
