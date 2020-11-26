using Microsoft.AspNetCore.Mvc;
using LetsDoStuff.Domain;
using System.Linq;
using LetsDoStuff.Domain.Models;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
        }

        [HttpGet]
        public string GetTestOutput()
        {
            return "test output";
        }


        [HttpGet("userinfo")]
        [Authorize]
        public ActionResult<User> GetUserInfo()
        {
            
            using (LdsContext db = new LdsContext())
            {
                var userLogin = this.HttpContext.User.Claims.FirstOrDefault().Value;
               
                
                var user = db.Users.FirstOrDefault(itm => itm.Login == userLogin);
                
                if (user == null)
                {
                    return BadRequest();
                }
                return user;
            }
        }
        [HttpGet("getuser")]
        [Authorize(Roles = "admin")]
        public ActionResult<User> GetUser(int id)
        {

            using (LdsContext db = new LdsContext())
            {

                var user = db.Users.FirstOrDefault(itm => itm.Id == id);

                if (user == null)
                {
                    return BadRequest();
                }
                return user;
            }
        }

        [HttpGet("get_all_users")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            
            using (LdsContext db = new LdsContext())
            {

                var users = db.Users.ToList();

                if (users == null)
                {
                    return BadRequest();
                }
                return users;
            }
        }
    }
}
