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
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly LdsContext context;
        private readonly IHubContext<ParticipationHub> _hubContext;

        public AccountController(LdsContext context, IHubContext<ParticipationHub> hubContext)
        {
            this.context = context;
            _hubContext = hubContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest userAuthParam)
        {
            var identity = GetIdentity(userAuthParam.Login, userAuthParam.Password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password" });
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthConstants.Issuer,
                audience: AuthConstants.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(AuthConstants.TokenLifetime),
                signingCredentials: new SigningCredentials(AuthConstants.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = context.Users.FirstOrDefault<User>(x => x.Email == login && x.Password == password)
                ?? context.Users.FirstOrDefault(itm => itm.ProfileLink == login);
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(AuthConstants.NameClaimType, user.Email),
                        new Claim(AuthConstants.RoleClaimType, user.Role),
                        new Claim(AuthConstants.IdClaimType, user.Id.ToString())
                    };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims);
                return claimsIdentity;
            }

            return null;
        }
    }
}