using System;
using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuthJwt;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateActivity([FromBody] CreateActivityCommand newActivity)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);

                _activityService.CreateActivity(newActivity, idUser);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
