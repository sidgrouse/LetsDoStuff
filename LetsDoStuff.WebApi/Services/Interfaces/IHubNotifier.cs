using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IHubNotifier
    {
        void NotifyAboutNewParticipationRequest();

        void NotifyAboutOwnerActivitiesAnswering();

        void NotifyAboutInvitations();
    }
}
