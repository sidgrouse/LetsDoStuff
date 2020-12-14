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
    [Route("api/subscribing")]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationService _subscribingService;

        public ParticipationController(IParticipationService subscribingService)
        {
            _subscribingService = subscribingService;
        }

        /// <summary>
        /// Take participation in an activity.
        /// </summary>
        /// <param name="idActivity">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpGet("SubscribeToActivity")]
        [Authorize]
        public IActionResult SubscribeToActivity(int idActivity)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);
                _subscribingService.MakeUserSubscribedToActivityByIds(idUser, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refuse to participation in an Activity.
        /// </summary>
        /// <param name="idActivity">The Id of an Activity.</param>
        /// <returns>Action result.</returns>
        [HttpGet("UnsubscribeToActivity")]
        [Authorize]
        public IActionResult UnsubscribeToActivity(int idActivity)
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);
                _subscribingService.MakeUserUnsubscribedToActivityByIds(idUser, idActivity);
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
        /// <returns>Action result.</returns>
        [HttpGet("subscriberinfo")]
        [Authorize]
        public ActionResult<List<ActivityResponse>> GetSubscriberInfo()
        {
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == UserClaimIdentity.DefaultIdClaimType)
                    .First().Value);
                var subscriberinfo = _subscribingService.GetUsersParticipations(idUser);
                return subscriberinfo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
