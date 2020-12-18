using System;
using System.Linq;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger<IUserService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get a user settings.
        /// </summary>
        /// <returns>A specified user settings.</returns>
        [Authorize]
        [HttpGet("settings")]
        public ActionResult<UserSettingsResponse> GetUserSettings()
        {
            var idUser = int.Parse(this.HttpContext.User.Claims
                    .Where(c => c.Type == AuthConstants.IdClaimType)
                    .First().Value);
            try
            {
                var user = _userService.GetUserSettings(idUser);
                return user;
            }
            catch (ArgumentException ae)
            {
                return NotFound(ae.Message);
            }
        }

        /// <summary>
        /// Get a specific user.
        /// </summary>
        /// <param name="profilelink">A User profile link.</param>
        /// <returns>A specified user.</returns>
        [Authorize]
        [HttpGet("{profilelink}")]
        public ActionResult<UserResponse> GetUserByUserProfile(string profilelink)
        {
            try
            {
                var user = _userService.GetUserByProfileLink(profilelink);
                return user;
            }
            catch (ArgumentException ae)
            {
                return NotFound(ae.Message);
            }
        }

        /// <summary>
        /// Register user and generate the profileLink with identity starts 1. "user1", "user2" e.g.
        /// </summary>
        /// <param name="request">User registration data.</param>
        /// /// <returns>Action result.</returns>
        [HttpPost("registration")]
        public IActionResult Registration([FromBody]RegisterRequest request)
        {
            try
            {
                _userService.RegisterUser(request);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now}");
            }

            return Ok();
        }
    }
}
