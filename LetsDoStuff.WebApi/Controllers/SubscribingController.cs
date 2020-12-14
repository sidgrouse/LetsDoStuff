using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.WebApi.Services;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/subscribing")]
    public class SubscribingController : ControllerBase
    {
        private readonly ISubscribingService _subscribingService;

        public SubscribingController(ISubscribingService subscribingService)
        {
            _subscribingService = subscribingService;
        }

        [HttpGet("SubscribeToActivity")]
        [Authorize]
        public IActionResult SubscribeToActivity(int idActivity)
        {
            var loginName = this.HttpContext.User.Claims.First().Value;
            try
            {
                _subscribingService.MakeUserSubscribedToActivityByEmailAndId(loginName, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UnsubscribeToActivity")]
        [Authorize]
        public IActionResult UnsubscribeToActivity(int idActivity)
        {
            var loginName = this.HttpContext.User.Claims.First().Value;
            try
            {
                _subscribingService.MakeUserUnsubscribedToActivityByEmailAndId(loginName, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SubscribeToActivityById")]
        [Authorize]
        public IActionResult SubscribeToActivityById(int idUser, int idActivity)
        {
            try
            {
                _subscribingService.MakeUserSubscribedToActivityByIds(idUser, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UnsubscribeToActivityById")]
        [Authorize]
        public IActionResult UnsubscribeToActivityById(int idUser, int idActivity)
        {
            try
            {
                _subscribingService.MakeUserUnsubscribedToActivityByIds(idUser, idActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subscriberinfo")]
        [Authorize]
        public ActionResult<List<ActivityResponse>> GetSubscriberInfo()
        {
            var userName = this.HttpContext.User.Claims.FirstOrDefault().Value;
            var subscriberinfo = _subscribingService.GetAllActivitiesOfTheUser(userName);
            return subscriberinfo;
        }
    }
}
