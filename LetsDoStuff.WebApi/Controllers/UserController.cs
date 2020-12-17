using System;
using System.Collections.Generic;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
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
        /// Get list of all users. To access this you need to have the "Admin" role.
        /// </summary>
        /// <returns>All users.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public List<UserSettingsResponse> GetAllUsers()
        {
            var users = _userService.GetAllUsers();

            return users;
        }

        /// <summary>
        /// Get a user settings.
        /// </summary>
        /// <returns>A specified user settings.</returns>
        [Authorize]
        [HttpGet("settings")]
        public ActionResult<UserSettingsResponse> GetUserSettings()
        {
            var email = HttpContext.User.Identity.Name;
            try
            {
                var user = _userService.GetUserSettings(email);
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
