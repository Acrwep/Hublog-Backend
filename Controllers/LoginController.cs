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

        #region  User Login
        //[AllowAnonymous]
        //[HttpPost]

        //public HttpResponseMessage UserLogin(LoginModels model)
        //{
        //    HttpResponseMessage response = null;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var result = Task.FromResult(_dapper.GetAll<Users>("select A.*,B.Name as RoleName,B.AccessLevel,C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  where B.AccessLevel=2 and A.Email='" + model.UserName + "' and A.Password='" + model.Password + "' and A.Active=1").ToList());
        //            if (result.IsCompleted)
        //            {
        //                if (result.Result.Count != 0)
        //                {
        //                    var token = CommonFunctiton.CreateToken(result.Result[0]);
        //                    response = Request.CreateResponse(new { user = result.Result[0], token = token });
        //                }
        //                else
        //                {
        //                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
        //                }
        //            }
        //            else
        //            {
        //                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invaild UserName & Password");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logErrors.Writelog(ex, "Login", "UserLogin");
        //            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
        //        }
        //    }
        //    else
        //    {
        //        _logErrors.WriteDirectLog("Login", "UserLogin" + "Model State is Not Valid");
        //        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
        //    }
        //    return response;
        //}
        #endregion


        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage UserLogin(LoginModels model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                try
                {
                    string query = "SELECT A.*, B.Name AS RoleName, B.AccessLevel, C.Name AS DesignationName, D.Name AS TeamName " +
                                   "FROM Users A WITH (NOLOCK) " +
                                   "INNER JOIN Role B WITH (NOLOCK) ON A.RoleId = B.Id " +
                                   "INNER JOIN Designation C WITH (NOLOCK) ON A.DesignationId = C.Id " +
                                   "INNER JOIN Team D WITH (NOLOCK) ON A.TeamId = D.Id " +
                                   "WHERE B.AccessLevel = 2 AND A.Email = @UserName AND A.Password = @Password AND A.Active = 1";
                    var parameters = new { UserName = model.UserName, Password = model.Password };
                    var result = _dapper.GetAll<Users>(query, parameters).ToList();

                    if (result.Any())
                    {
                        var token = CommonFunctiton.CreateToken(result[0]);
                        response = Request.CreateResponse(HttpStatusCode.OK, new { user = result[0], token });
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid UserName & Password");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Login", "UserLogin");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error occurred: " + ex.Message);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Login", "UserLogin Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        #region UserLogout
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
        #endregion

        #region AdminLogin
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage AdminLogin(LoginModels model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Users>("select A.*,B.Name as RoleName,B.AccessLevel," +
                        "C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  " +
                        "Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on" +
                        " A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  " +
                        "where B.AccessLevel=1 and  A.Email='" + model.UserName + "' and" +
                        " A.Password='" + model.Password + "' and A.Active=1").ToList());
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
        #endregion


        #region Login
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Login(LoginModels model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    // Updated query to include role details
                    var result = Task.FromResult(_dapper.GetAll<Users>("select A.*, B.Name as RoleName, B.AccessLevel, " +
                        "C.Name as DesignationName, D.Name as TeamName from Users A With(NoLock) " +
                        "inner join Role B With(NoLock) on A.RoleId = B.Id " +
                        "inner join Designation C With(NoLock) on A.DesignationId = C.Id " +
                        "inner join Team D With(NoLock) on A.TeamId = D.Id " +
                        "where A.Email = @Email and A.Password = @Password and A.Active = 1",
                        new { Email = model.UserName, Password = model.Password }).ToList());

                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            var user = result.Result[0];
                            var token = CommonFunctiton.CreateToken(user);

                            // Return the response with user details and token
                            response = Request.CreateResponse(new
                            {
                                user = new
                                {
                                    user.Id,
                                    user.First_Name,
                                    user.Last_Name,
                                    user.Email,
                                    user.DOB,
                                    user.DOJ,
                                    user.Phone,
                                    user.UsersName,
                                    user.Gender,
                                    user.OrganizationId,
                                    user.RoleId,
                                    user.DesignationId,
                                    user.TeamId,
                                    user.Active,
                                    RoleName = user.RoleName,
                                    AccessLevel = user.AccessLevel,
                                    DesignationName = user.DesignationName,
                                    TeamName = user.TeamName,
                                    EmployeeID = user.EmployeeID
                                },
                                token = token
                            });
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid UserName or Password");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid UserName or Password");
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
        #endregion

    }
}
