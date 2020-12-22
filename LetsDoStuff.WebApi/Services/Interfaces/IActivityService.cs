using System.Collections.Generic;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivityResponse> GetAllActivities();

        void CreateActivity(CreateActivityCommand newActivity, int userId);

        ActivityResponse GetActivityById(int activityId);
    }
}
