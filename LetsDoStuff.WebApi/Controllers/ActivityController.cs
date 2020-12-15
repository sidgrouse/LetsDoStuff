using System;
using System.Collections.Generic;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/activity")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        /// <summary>
        /// Get list of activities.
        /// </summary>
        /// <returns>All activities.</returns>
        [HttpGet("all")]
        public ActionResult<List<ActivityResponse>> GetActivities()
        {
            var activities = _activityService.GetAllActivities();

            if (activities == null)
            {
                return NotFound();
            }

            return activities;
        }

        /// <summary>
        /// Get a specific activity.
        /// </summary>
        /// <param name="id">ID of activity.</param>
        /// <returns>A specified activity.</returns>
        [HttpGet("{id}")]
        public ActionResult<ActivityResponse> GetActivity(int id)
        {
            var activity = _activityService.GetActivityById(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        /// <summary>
        /// Create activity.
        /// </summary>
        /// <param name="newActivity">Activity.</param>
        /// <returns>Action result.</returns>
        [HttpPost("create")]
        public IActionResult CreateActivity([FromBody]CreateActivityCommand newActivity)
        {
            try
            {
                _activityService.CreateActivity(newActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
