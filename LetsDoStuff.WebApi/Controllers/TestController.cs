using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private LdsContext db;

        public TestController(LdsContext context)
        {
            db = context;
        }

        [HttpGet]
        public string GetTestOutput()
        {
            return "test output";
        }

        [HttpGet("userinfo")]

        // [Authorize]
        public ActionResult<User> GetUserInfo()
        {
            var userLogin = this.HttpContext.User.Claims.FirstOrDefault().Value;

            var user = db.Users.FirstOrDefault(itm => itm.Login == userLogin);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("allusers")]

        // [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<User>> GetTestUsers()
        {
            return db.Users.ToList();
        }

        [HttpGet("user/{id}")]

        // [Authorize(Roles = "admin")]
        public ActionResult<User> GetUser(int id)
        {
            var user = db.Users.FirstOrDefault(itm => itm.Id == id);
            if (user == null)
            {
                return NotFound();
            }
                
            return user;
        }
    }
}
