using EMP;
using JWT.Exceptions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace EMP
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MessageHandlers.Add(new PreflightRequestsHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                   routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
    public class PreflightRequestsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                    CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;

            try
            {
                IEnumerable<string> authHeaderValues;
                request.Headers.TryGetValues("Authorization", out authHeaderValues);


                if (authHeaderValues == null)
                    return base.SendAsync(request, cancellationToken); // cross fingers

                var bearerToken = authHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                var secret = ConfigurationManager.AppSettings.Get("secret");

                Thread.CurrentPrincipal = ValidateToken(
                    token,
                    secret,
                    true
                    );

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = Thread.CurrentPrincipal;
                }
            }
            catch (SignatureVerificationException ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


            return errorResponse != null
                ? Task.FromResult(errorResponse)
                : base.SendAsync(request, cancellationToken);
        }

        private static ClaimsPrincipal ValidateToken(string token, string secret, bool checkExpiration)
        {
            var issuer = ConfigurationManager.AppSettings["issuer"];
            var audience = ConfigurationManager.AppSettings.Get("Audience");
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.RequireExpirationTime = checkExpiration;
            validationParameters.ValidAudience = audience.ToLower();
            validationParameters.ValidIssuer = issuer.ToLower();
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);
            var claims = principal.Claims.ToList();
            var exp = claims.First(c => c.Type == "exp").Value;
            if (claims != null && (checkExpiration && exp != null))
            {
                var validTo = FromUnixTime(long.Parse(exp.Replace(".0", "")));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception(
                        string.Format("Token is expired. Expiration: '{0}'. Current: '{1}'", validTo, DateTime.UtcNow));
                }
            }

            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);

            subject.AddClaims(claims);
            return new ClaimsPrincipal(subject);
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
