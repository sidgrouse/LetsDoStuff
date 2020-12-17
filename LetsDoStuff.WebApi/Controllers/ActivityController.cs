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
        public List<ActivityResponse> GetActivities()
        {
            var activities = _activityService.GetAllActivities();

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
            try
            {
                var activity = _activityService.GetActivityById(id);
                return activity;
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get list of available tags.
        /// </summary>
        /// <returns>All available tags.</returns>
        [HttpGet("tags")]
        public List<TagResponse> GetAvailableTags()
        {
            var tags = _activityService.GetAvailableTags();

            return tags;
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
                return BadRequest(ex.Message);
            }
        }
    }
}
