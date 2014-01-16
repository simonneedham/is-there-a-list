using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security.Google;

namespace IsThereAList
{
    public class GoogleExtraScopesProvider : GoogleAuthenticationProvider
    {
        public override Task Authenticated(GoogleAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("profileClaim", @"https://www.googleapis.com/auth/userinfo.profile"));
            context.Identity.AddClaim(new Claim("emailClaim", @"https://www.googleapis.com/auth/userinfo.email"));
            return base.Authenticated(context);
        }
    }
}