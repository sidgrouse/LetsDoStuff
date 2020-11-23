using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
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
        public IEnumerable<User> GetTestOutput()
        {
            return db.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
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
