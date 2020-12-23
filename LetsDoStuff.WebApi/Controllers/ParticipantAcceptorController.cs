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
    [Route("api/ParticipantAcceptor")]
    public class ParticipantAcceptorController : ControllerBase
    {
        private readonly IParticipantAcceptorService _participationAccepter;

        public ParticipantAcceptorController(IParticipantAcceptorService participationAccepter)
        {
            _participationAccepter = participationAccepter;
        }

        /// <summary>
        /// Get info about all users who want to join to your Activitirs.
        /// </summary>
        /// <returns>All info about participations that helps a creator to accept or reject users.</returns>
        [Authorize]
        [HttpGet]
        public ActionResult<List<ParticipationResponseForCreator>> GetParticipantesInfo()
        {
            try
            {
                var info = _participationAccepter.GetParticipationsInfo(UserId);

                return info;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Accept a user as Participant of an activity.
        /// </summary>
        /// <param name="acceptReject">The Id of a Activity and the Id of a participant.</param>
        /// <returns>Action result.</returns>
        [Authorize]
        [HttpPut]
        public IActionResult AcceptParticipant(AcceptRejectRequest acceptReject)
        {
            try
            {
                _participationAccepter.AcceptParticipant(UserId, acceptReject.ActivityId, acceptReject.ParticipanteId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reject a user as Participant of an activity.
        /// </summary>
        /// <param name="rejectRequest">The Id of a Activity and the Id of a participant.</param>
        /// <returns>Action result.</returns>
        [Authorize]
        [HttpDelete]
        public IActionResult RejectParticipant(AcceptRejectRequest rejectRequest)
        {
            try
            {
                _participationAccepter.RejectParticipant(UserId, rejectRequest.ActivityId, rejectRequest.ParticipanteId);
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
