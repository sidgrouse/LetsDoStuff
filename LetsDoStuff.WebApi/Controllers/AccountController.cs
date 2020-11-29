using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private LdsContext context;

        public AccountController(LdsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public string GetTestOutput()
        {
            return "test output";
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthParam userAuthParam)
        {
            var identity = GetIdentity(userAuthParam.Login, userAuthParam.Password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or passeword" });
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
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
            var user = context.Users.FirstOrDefault<User>(x => x.Login == login && x.Password == passeword);
            if (user != null)
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
