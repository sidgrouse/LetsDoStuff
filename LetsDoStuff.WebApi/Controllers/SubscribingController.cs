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

        [HttpGet("SubscribeToActivityByName")]
        [Authorize]
        public IActionResult SubscribeToActivityByLogin(string nameActivity)
        {
            var loginName = this.HttpContext.User.Claims.First().Value;
            try
            {
                _subscribingService.SubscribeUserToActivityByNames(loginName, nameActivity);
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
