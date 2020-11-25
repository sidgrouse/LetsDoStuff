using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using LetsDoStuff.WebApi.Authentefication;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        [HttpGet]
        public string GetTestOutput()
        {
            return "test output";
        }

        public AccountController()
        {

        }
        [HttpPost("token")]
        public IActionResult Token(string userLogin, string userPasseword)
        {
            var identity = GetIdentity(userLogin, userPasseword);

            if(identity==null)
            {
                return BadRequest(new { errorText="Invalid login or passeword" } );
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore:now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256) );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                login = identity.Name
            };

            return Json(response);
        }

      

        private ClaimsIdentity GetIdentity(string login, string passeword)
        {
            using (LdsContext bd = new LdsContext())
            {
                var user = bd.Users.FirstOrDefault<User>(x => x.Login == login && x.Passeword == passeword);
                if(user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                    };
                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }

                return null;
            }
        }
    }
}
