using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.WebApi.Services;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuthJwt;
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
        /// <param name="idActivity">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpGet("AddParticipation")]
        [Authorize]
        public IActionResult AddParticipation(int idActivity)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);
                _participationService.AddParticipation(idUser, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove an Activity from User's Participations.
        /// </summary>
        /// <param name="idActivity">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpGet("RemoveParticipation")]
        [Authorize]
        public IActionResult RemoveParticipation(int idActivity)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);
                _participationService.RemoveParticipation(idUser, idActivity);
                return Ok();
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
        [HttpGet("GetUsersParticipations")]
        [Authorize]
        public ActionResult<List<ActivityResponse>> GetUsersParticipations()
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
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
