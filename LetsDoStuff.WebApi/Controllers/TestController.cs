using Microsoft.AspNetCore.Mvc;
using LetsDoStuff.Domain;
using System.Linq;
using LetsDoStuff.Domain.Models;

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
