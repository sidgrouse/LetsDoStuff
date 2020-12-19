using System;
using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/participation")]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService)
        {
            _participationService = participationService;
        }

        /// <summary>
        /// Add an Activity to User's Participations.
        /// </summary>
        /// <param name="request">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddParticipation(ParticipationRequest request)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
                _participationService.AddParticipation(idUser, request.IdActivity);
                return Ok(new { result = "Activity successfully added to user`s Participations." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove an Activity from User's Participations.
        /// </summary>
        /// <param name="request">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpDelete]
        [Authorize]
        public IActionResult RemoveParticipation(ParticipationRequest request)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
                _participationService.RemoveParticipation(idUser, request.IdActivity);
                return Ok(new { result = "Activity successfully removed from user`s Participations." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all user's Activities for participation.
        /// </summary>
        /// <returns>All user's activities for Participations.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult<List<ActivityResponse>> GetUsersParticipations()
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
                var userinfo = _participationService.GetUsersParticipations(idUser);
                return userinfo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
