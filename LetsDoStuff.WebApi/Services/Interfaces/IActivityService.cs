using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivitiesResponse> GetAllActivities();

        void CreateActivity(CreateActivityCommand newActivity, int userId);

        void DeleteActivity(int activityId, int userId);

        List<TagResponse> GetAvailableTags();

        ActivityResponse GetActivityById(int activityId);
    }
}
