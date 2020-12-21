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
        public List<MayParticipationResponse> GetAllParticipantes(int activityId)
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            var activities = _acceptionService.GetAcrivityMayParticipantes(idUser, activityId);

            return activities;
        }

        [Authorize]
        [HttpPut]
        public IActionResult Accept(int activityId, int participanteId)
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            _acceptionService.Accept(idUser, activityId, participanteId);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Reject(int activityId, int participanteId)
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            _acceptionService.Reject(idUser, activityId, participanteId);
            return Ok();
        }
    }
}
