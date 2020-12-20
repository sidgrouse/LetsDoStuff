using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
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

        public List<UserResponse> GetActivities()
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            var activities = _acceptionService.GetAllParticipantes();

            return activities;
        }

        public IActionResult Accept(int participanteId)
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            _acceptionService.Accept(participanteId);
            return Ok();
        }

        public IActionResult Reject(int participanteId)
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            _acceptionService.Reject(participanteId);
            return Ok();
        }
    }
}
