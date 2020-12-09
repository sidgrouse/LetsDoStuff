using System;
using LetsDoStuff.Domain.Models.DTO;
using LetsDoStuff.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserContoller : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserContoller(IUserService userService, ILogger<UserService> logger)
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
            var userName = HttpContext.User.Identity.Name;
            try
            {
                var user = _userService.GetUserSettings(userName);
                return user;
            }
            catch (ArgumentException ae)
            {
                return NotFound(ae.Message);
            }
        }

        /// <summary>
        /// Get a user by userName.
        /// </summary>
        /// <param name="userName">UserName of User.</param>
        /// <returns>A specified user.</returns>
        [Authorize]
        [HttpGet("{userName}")]
        public ActionResult<UserResponse> GetUserByUserName(string userName)
        {
            try
            {
                var user = _userService.GetUserByUserName(userName);
                return user;
            }
            catch (ArgumentException ae)
            {
                return NotFound(ae.Message);
            }
        }

        /// <summary>
        /// Register a user and generate the userName with identity starts 1. "user1", "user2" e.g.
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
                _logger.LogError(ae, $"{DateTime.Now}");
                return BadRequest(ae.Message);
            }

            return Ok();
        }
    }
}
