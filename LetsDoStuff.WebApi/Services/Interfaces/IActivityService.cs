using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivityResponse> GetAllActivities();

        void CreateActivity(CreateActivityCommand newActivity, int idUser);

        List<TagResponse> GetAvailableTags();

        ActivityResponse GetActivityById(int activityId);
    }
}
