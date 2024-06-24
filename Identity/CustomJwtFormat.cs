using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace EMP.Identity
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {

        private static readonly byte[] _secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);
        private readonly string _issuer;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["secret"]));
            var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(_issuer, "Any", data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey));
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}