using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
            var activities = db.Activities;

            return activities.ToList();
        }

        /// <summary>
        /// Get a specific activity.
        /// </summary>
        /// <param name="id">ID of activity.</param>
        /// <returns>A specified activity.</returns>
        [HttpGet("activity")]
        public ActionResult<Activity> GetActivity(int id)
        {
            var activity = db.Activities.FirstOrDefault(itm => itm.Id == id);

            if (activity == null)
            {
                return BadRequest();
            }

            return activity;        
        }

        /// <summary>
        /// Create an activity.
        /// </summary>  
        /// <returns>A newly created activity.</returns>
        [HttpPost("activity")]
        public ActionResult<Activity> CreateActivity()
        {
            var activity = new Activity();
            return activity;
        }
    }
}
