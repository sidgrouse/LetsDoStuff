using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/activities")]
    public class ActivityController : ControllerBase
    {
        private LdsContext db;

        public ActivityController(LdsContext context)
        {
            db = context;
        }

        /// <summary>
        /// Get list of activities.
        /// </summary>
        /// <returns>All activities.</returns>
        [HttpGet]
        public ActionResult<ICollection<Activity>> GetActivityOutput()
        {
            var activities = db.Activities.Include(a => a.Tags).ToList();

            return activities;
        }

        /// <summary>
        /// Get a specific activity.
        /// </summary>
        /// <param name="id">ID of activity.</param>
        /// <returns>A specified activity.</returns>
        [HttpGet("activity")]
        public ActionResult<Activity> GetActivity(int id)
        {
            var activity = db.Activities.Include(a => a.Tags).FirstOrDefault(itm => itm.Id == id);

            if (activity == null)
            {
                return BadRequest();
            }

            return activity;        
        }

        /// <summary>
        /// Create an activity.
        /// </summary>  
        /// <param name="activityRequest">Activity.</param>
        /// <returns>A newly created activity.</returns>
        [HttpPost("activity")]
        public ActionResult<Activity> CreateActivity(CreateActivityRequest activityRequest)
        {
            var activity = new Activity();
            activity.Name = activityRequest.Name;
            activity.Description = activityRequest.Description;
            activity.Capacity = activityRequest.Capacity;
            activity.Id_Creator = activityRequest.IdCreator;

            activity.Creator = db.Users.Find(activityRequest.IdCreator);

            foreach (int id in activityRequest.TagIds)
            {
                activity.Tags.Add(db.Tags.Find(id));
            }

            db.Activities.Add(activity);
            db.SaveChanges();

            return activity;
        }
    }
}
