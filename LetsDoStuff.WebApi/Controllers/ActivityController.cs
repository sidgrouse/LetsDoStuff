using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
using LetsDoStuff.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api")]
    public class ActivityController : ControllerBase
    {
        private ActivityManager am;

        public ActivityController(ActivityManager activityManager)
        {
            am = activityManager;
        }

        /// <summary>
        /// Get list of activities.
        /// </summary>
        /// <returns>All activities.</returns>
        [HttpGet("activities")]
        public ActionResult<List<Activity>> GetActivities()
        {
            var activities = am.GetActivities();

            if (activities == null)
            {
                return NotFound();
            }

            return Ok(activities);
        }

        /// <summary>
        /// Get a specific activity.
        /// </summary>
        /// <param name="id">ID of activity.</param>
        /// <returns>A specified activity.</returns>
        [HttpGet("{id}")]
        public ActionResult<Activity> GetActivity(int id)
        {
            var activity = am.GetActivityById(id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        /// <summary>
        /// Create activity.
        /// </summary>
        /// <param name="newActivity">Activity.</param>
        /// <returns>Action result.</returns>
        [HttpPost("createactivity")]
        public IActionResult CreateActivity(CreateActivityCommand newActivity)
        {
            return am.CreateActivity(newActivity);
        }
    }
}
