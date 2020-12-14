using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LetsDoStuff.WebApi.SettingsForAuthJwt
{
    public static class UserClaimIdentity
    {
        public const string DefaultNameClaimType = ClaimsIdentity.DefaultNameClaimType;
        public const string DefaultRoleClaimType = ClaimsIdentity.DefaultRoleClaimType;
        public const string DefaultIdClaimType = "Id";
    }
}
