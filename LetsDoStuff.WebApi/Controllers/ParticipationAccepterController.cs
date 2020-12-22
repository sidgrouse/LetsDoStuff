using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/ParticipationAcception")]
    public class ParticipationAccepterController : ControllerBase
    {
        private readonly IParticipationAccepterService _participationAccepter;

        public ParticipationAccepterController(IParticipationAccepterService participationAccepter)
        {
            _participationAccepter = participationAccepter;
        }

        /// <summary>
        /// Get Creator's info about all may participants that could help you with acception and Rejection.
        /// </summary>
        /// <returns>All information about participations that helps a creator to accept/reject users.</returns>
        [Authorize]
        [HttpGet]
        public ActionResult<List<ParticipationResponseForCreator>> GetActivityParticipantes()
        {
            try
            {
                var activities = _participationAccepter.GetAllParticipations(UserId);

                return activities;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Accept a user as Participant.
        /// </summary>
        /// <param name="acceptReject">The Id of a Activity and the Id of a participant.</param>
        /// <returns>Action result.</returns>
        [Authorize]
        [HttpPut]
        public IActionResult AcceptParticipant(AcceptRejectRequest acceptReject)
        {
            try
            {
                _participationAccepter.AcceptParticipation(UserId, acceptReject.ActivityId, acceptReject.ParticipanteId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reject a user as Participant.
        /// </summary>
        /// <param name="rejectRequest">The Id of a Activity and the Id of a participant.</param>
        /// /// <returns>Action result.</returns>
        [Authorize]
        [HttpDelete]
        public IActionResult RejectParticipant(AcceptRejectRequest rejectRequest)
        {
            try
            {
                _participationAccepter.RejectParticipation(UserId, rejectRequest.ActivityId, rejectRequest.ParticipanteId);
                return Ok();
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
