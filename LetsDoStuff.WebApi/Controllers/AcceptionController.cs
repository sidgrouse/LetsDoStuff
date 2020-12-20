using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
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
            var activities = _acceptionService.GetAllParticipantes();

            return activities;
        }

        public IActionResult Accept(int participanteId)
        {
            _acceptionService.Accept(participanteId);
            return Ok();
        }

        public IActionResult Reject(int participanteId)
        {
            _acceptionService.Reject(participanteId);
            return Ok();
        }
    }
}
