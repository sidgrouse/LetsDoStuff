using Microsoft.AspNetCore.Mvc;
using LetsDoStuff.Domain;
using System.Linq;
using LetsDoStuff.Domain.Models;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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


        [HttpGet("user")]
        [Authorize]
        public ActionResult<User> GetUser()
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

    }
}
