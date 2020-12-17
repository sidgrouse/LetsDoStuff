using System.Collections.Generic;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivityResponse> GetAllActivities();

        List<TagResponse> GetAvailableTags();

        void CreateActivity(CreateActivityCommand newActivity);

        ActivityResponse GetActivityById(int activityId);
    }
}
