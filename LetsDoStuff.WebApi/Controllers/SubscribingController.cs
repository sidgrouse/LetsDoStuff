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
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == "Id")
                    .First().Value);
                _subscribingService.MakeUserSubscribedToActivityByIds(idUser, idActivity);
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
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == "Id")
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
            try
            {
                var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == "Id")
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
