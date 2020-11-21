using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
