using JWT;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using EMP;
using EMP.Models.Master;
using System.Linq;
using EMP.Controllers;

namespace EMP.AppControllers.key
{
    public class LoginController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();


        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage UserLogin(LoginModels model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Users>("select A.*,B.Name as RoleName,B.AccessLevel,C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  where B.AccessLevel=2 and A.Email='" + model.UserName + "' and A.Password='" + model.Password + "' and A.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            var token = CommonFunctiton.CreateToken(result.Result[0]);
                            response = Request.CreateResponse(new { user =result.Result[0],token= token });
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Login", "UserLogin");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Login", "UserLogin"+ "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage UserLogout(LoginModels model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Users>("select *,B.Name as RoleName,B.AccessLevel,C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  where B.AccessLevel=2 and A.Email='" + model.UserName + "' and A.Password='" + model.Password + "' and A.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            var token = CommonFunctiton.CreateToken(result.Result[0]);
                            response = Request.CreateResponse(new { user = result.Result[0], token = token });
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Login", "UserLogout");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Login", "UserLogout" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage AdminLogin(LoginModels model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Users>("select A.*,B.Name as RoleName,B.AccessLevel,C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  where B.AccessLevel=1 and  A.Email='" + model.UserName + "' and A.Password='" + model.Password + "' and A.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            var token = CommonFunctiton.CreateToken(result.Result[0]);
                            response = Request.CreateResponse(new { user = result.Result[0], token = token });
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Login", "UserLogin");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Login", "UserLogin" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }


    }
}
