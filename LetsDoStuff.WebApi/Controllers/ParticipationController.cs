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
        private readonly IHubNotifier _hubNotifier;

        public ParticipationController(IParticipationService participationService, IHubNotifier hubNotifier)
        {
            _participationService = participationService;
            _hubNotifier = hubNotifier;
        }

        /// <summary>
        /// Get user's info about activities for participation.
        /// </summary>
        /// <returns>All information about user's participations.</returns>
        [Authorize]
        [HttpGet("activitiesinfo")]
        public ActionResult<List<ParticipationResponse>> GetParticipationInfo()
        {
            try
            {
                var info = _participationService.GetParticipationInfo(UserId);
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
        [Authorize]
        [HttpPost("add")]
        public IActionResult AddParticipation(ParticipationRequest request)
        {
            try
            {
                _participationService.AddParticipation(UserId, request.ActivityId);
                _hubNotifier.NotifyAboutNewParticipationRequest(request.ActivityId, UserId);
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
        [Authorize]
        [HttpDelete("remove")]
        public IActionResult RemoveParticipation(ParticipationRequest request)
        {
            try
            {
                _participationService.RemoveParticipation(UserId, request.ActivityId);
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
        /// Get info about all users who want to join to your Activities.
        /// </summary>
        /// <returns>All info about participations that helps a creator to accept or reject users.</returns>
        [Authorize]
        [HttpGet("participantsinfo")]
        public ActionResult<List<ParticipantResponse>> GetParticipantsInfo()
        {
            try
            {
                var info = _participationService.GetParticipantInfo(UserId);

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
        [HttpPut("accept")]
        public IActionResult AcceptParticipant(ParticipationResolutionRequest acceptReject)
        {
            try
            {
                _participationService.AcceptParticipant(UserId, acceptReject.ActivityId, acceptReject.ParticipantId);
                _hubNotifier.NotifyAboutOwnerActivitiesAnswering(acceptReject.ActivityId, acceptReject.ParticipantId, true);
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
        [HttpDelete("reject")]
        public IActionResult RejectParticipant(ParticipationResolutionRequest rejectRequest)
        {
            try
            {
                _participationService.RejectParticipant(UserId, rejectRequest.ActivityId, rejectRequest.ParticipantId);
                _hubNotifier.NotifyAboutOwnerActivitiesAnswering(rejectRequest.ActivityId, rejectRequest.ParticipantId, false);
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
