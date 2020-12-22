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

        [Authorize]
        [HttpPut]
        public IActionResult AcceptParticipant(int activityId, int participanteId)
        {
            try
            {
                _participationAccepter.AcceptParticipation(UserId, activityId, participanteId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
  
        [Authorize]
        [HttpDelete]
        public IActionResult RejectParticipant(int activityId, int participanteId)
        {
            try
            {
                _participationAccepter.RejectParticipation(UserId, activityId, participanteId);
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
