using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
//using EMP.Core;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using EMP.Models.Master;

namespace EMP.Identity
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly Dapperr _dapper = new Dapperr();
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var result = Task.FromResult(_dapper.GetAll<RoleModel>("select Name Role  With(NoLock)").ToList());
            var ticket = new AuthenticationTicket(SetClaimsIdentity(context, result.Result), new AuthenticationProperties());
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private static System.Security.Claims.ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, List<RoleModel> user)
        {
            var identity = new System.Security.Claims.ClaimsIdentity();
            identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, context.UserName));
            identity.AddClaim(new System.Security.Claims.Claim("sub", context.UserName));
            //foreach (var role in user)
            //{
            //    identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role.Name));
            //}
            return identity;
        }
    }
}