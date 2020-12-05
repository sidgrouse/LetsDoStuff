using System;
using System.Threading.Tasks;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Controllers
{
    public class ActivitesControllers : Controller
    {
        private LdsContext dbActives;

        public ActivitesControllers(LdsContext active) 
        {
            dbActives = active;
        }

        public IActionResult GetActivites()
        {
            return (IActionResult)dbActives.Activites.ToListAsync();
        }
    }
}
