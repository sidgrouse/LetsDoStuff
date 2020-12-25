using System;
using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        /// <summary>
        /// Get list of activities ordered by date.
        /// </summary>
        /// <returns>All activities.</returns>
        [HttpGet]
        [EnableQuery(EnsureStableOrdering = false, AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.Top | AllowedQueryOptions.Skip, PageSize = 20)]
        public List<ActivitiesResponse> GetActivities()
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
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateActivity([FromBody] CreateActivityCommand newActivity)
        {
            try
            {
                _activityService.CreateActivity(newActivity, UserId);
                return Ok(new { result = "Activity successfully created." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete activity.
        /// </summary>
        /// <param name="activityId">ID of activity.</param>
        /// <returns>Action result.</returns>
        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteActivity(int activityId)
        {
            try
            {
                _activityService.DeleteActivity(activityId, UserId);
                return Ok(new { result = "Activity successfully deleted." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int UserId => int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
    }
}
