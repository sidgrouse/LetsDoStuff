using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain.Models.DTO;
using LetsDoStuff.WebApi.Services;
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
        public IActionResult SubscribeToActivityByName(string nameActivity)
        {
            var userName = this.HttpContext.User.Claims.FirstOrDefault().Value;
            try
            {
                _subscribingService.SubscribeUserToActivityByNames(userName, nameActivity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("subscriberinfo")]
        [Authorize]
        public ActionResult<List<ActivityResponse>> GetSubscriberInfo()
        {
            var userName = this.HttpContext.User.Claims.FirstOrDefault().Value;
            var subscriberinfo = _subscribingService.GetAllActivitiesOfTheUser(userName);
            if (subscriberinfo.Count == 0)
            {
                return NotFound();
            }

            return subscriberinfo;
        }
    }
}
