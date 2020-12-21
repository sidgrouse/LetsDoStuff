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
    [Route("api/acception")]
    public class AcceptionController : ControllerBase
    {
        private readonly IAcceptionService _acceptionService;

        public AcceptionController(IAcceptionService acceptionService)
        {
            _acceptionService = acceptionService;
        }

        [Authorize]
        [HttpGet("All")]
        public ActionResult<List<MayParticipationResponse>> GetAllParticipantes(int activityId)
        {
            try
            {
                var activities = _acceptionService.GetAcrivityMayParticipantes(IdUser, activityId);

                return activities;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Accept(int activityId, int participanteId)
        {
            try
            {
                _acceptionService.Accept(IdUser, activityId, participanteId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Reject(int activityId, int participanteId)
        {
            try
            {
                _acceptionService.Reject(IdUser, activityId, participanteId);
                return Ok();
            }
           catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int IdUser => int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
    }
}
