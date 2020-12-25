using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivitiesResponse> GetAllActivities();

        void CreateActivity(CreateActivityCommand newActivity, int idUser);

        void DeleteActivity(int userId, int activityId);

        List<TagResponse> GetAvailableTags();

        ActivityResponse GetActivityById(int activityId);
    }
}
