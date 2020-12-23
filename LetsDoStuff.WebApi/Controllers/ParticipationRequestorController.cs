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
    [Route("api/ParticipationRequestor")]
    public class ParticipationRequestorController : ControllerBase
    {
        private readonly IParticipationRequesterService _participationRequester;

        public ParticipationRequestorController(IParticipationRequesterService participationRequester)
        {
            _participationRequester = participationRequester;
        }

        /// <summary>
        /// Get user's info about activities for participation.
        /// </summary>
        /// <returns>All information about user's participations.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult<List<ParticipationResponse>> GetParticipationInfo()
        {
            try
            {
                var info = _participationRequester.GetParticipationInfo(UserId);
                return info;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                _participationRequester.AddParticipation(UserId, request.ActivityId);
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
                _participationRequester.RemoveParticipation(UserId, request.ActivityId);
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

        private int UserId => int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
    }
}
