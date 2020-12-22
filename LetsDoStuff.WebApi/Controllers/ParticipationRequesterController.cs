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
    [Route("api/ParticipationRequester")]
    public class ParticipationRequesterController : ControllerBase
    {
        private readonly IParticipationRequesterService _participationRequester;

        public ParticipationRequesterController(IParticipationRequesterService participationRequester)
        {
            _participationRequester = participationRequester;
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
                _participationRequester.AddParticipation(idUser, request.IdActivity);
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
                _participationRequester.RemoveParticipation(idUser, request.IdActivity);
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
        [HttpGet]
        [Authorize]
        public ActionResult<List<ParticipationResponseForUser>> GetUsersParticipations()
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
                var userinfo = _participationRequester.GetUsersParticipations(idUser);
                return userinfo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
