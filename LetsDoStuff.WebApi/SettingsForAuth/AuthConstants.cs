using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoStuff.WebApi.SettingsForAuth
{
    public static class AuthConstants
    {
        public const string Issuer = "MyAuthServer";
        public const string Audience = "MyAuthClient";
        public const string NameClaimType = "Name";
        public const string RoleClaimType = "Role";
        public const string IdClaimType = "Id";
        public const string UserRoleName = "User";
        public const string AdminRoleName = "Admin";

        public static TimeSpan TokenLifetime => TimeSpan.FromMinutes(20);

        public static SymmetricSecurityKey SymmetricSecurityKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));

        private const string KEY = "mysupersecret_secretkey!123";
    }
}
