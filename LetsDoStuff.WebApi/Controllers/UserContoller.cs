using System;
using LetsDoStuff.Domain.Models.DTO;
using LetsDoStuff.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserContoller : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger _logger;

        public UserContoller(UserService userService, ILogger<UserService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{userName}")]
        public ActionResult<GetUserByUserNameQueryResult> GetUserByUserName(string userName)
        {
            try
            {
                var user = _userService.GetUserByUserName(userName);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"User {userName} is not found.");
                return NotFound(new { Message = "User with this userName is not found." });
            }
        }

        [HttpPost("registration")]
        public IActionResult CreateUser([FromBody]CreateUserCommand request)
        {
            try
            {
                _userService.CreateUser(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"User is not created.");
                return BadRequest(new { Message = "Error!" });
            }

            return Ok(new { Message = "Success!" });
        }
    }
}
