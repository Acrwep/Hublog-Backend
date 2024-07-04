using EMP.Models.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EMP.Controllers
{
    public class AdminController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();



       
        [HttpPost]
        public HttpResponseMessage GetBreakMaster(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<BreakMaster>("select * from BreakMaster B with(Nolock) where B.OrganizationId="+ obj.OrganizationId + " and B.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "GetBreakMaster");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "GetBreakMaster" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage GetTeamMaster(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Team>("select * from Team B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "GetTeamMaster");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "GetTeamMaster" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        //[Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpGet]
        public HttpResponseMessage GetDesignationMaster(GetModels obj)
        {
           
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Designation>("select * from Designation B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.Active=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "GetDesignationMaster");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "GetDesignationMaster" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }
        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage GetRoleMaster(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Role>("select * from Role B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + "").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "GetRoleMaster");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "GetRoleMaster" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpPost]
        public HttpResponseMessage GetUsers(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Users>("select A.*,B.Name as RoleName,B.AccessLevel,C.Name as DesignationName,D.Name as TeamName from Users A With(NoLock)  inner join  Role B With(NoLock) on A.RoleId=B.Id   inner join  Designation C With(NoLock) on A.DesignationId=C.Id   inner join  Team D With(NoLock) on A.TeamId=D.Id  where A.OrganizationId=" + obj.OrganizationId + " and A.Active=1 and B.AccessLevel!=1").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "GetUsers");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Login", "GetUsers" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }


        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage GetScreenShots(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var result = Task.FromResult(_dapper.GetAll<UserScreenShotModel>("select * from ScreenShot B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.ServerDate between '"+obj.CDate+ "' and '" + obj.CDate + "' and B.UserId=" + obj.UserId+" ").ToList());
                    if (result.IsCompleted)
                    {
                        if (result.Result.Count != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "GetScreenShots");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "GetScreenShots" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage GetMonthlyinout(GetModels obj)
        {
            #region====InitateVariables====
            ResponseViewModel Ro = new ResponseViewModel();
            Ro.status = false;
            Ro.status_code = Convert.ToInt32(EnumManager.Status.Error);
            Ro.message = Message.NotFound;
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            #endregion
            #region=======Logic======
            try
            {
                var result = Task.FromResult(_dapper.GetAll<MontinoutModel>("EXEC [dbo].[SP_Monthinout] " + obj.UserId + "," + obj.OrganizationId + ",'" + obj.FDate + "','" + obj.TDate + "'").ToList());
                if (result.IsCompleted)
                {
                    if (result.Result.Count != 0)
                    {
                        Ro.data = result.Result;
                        Ro.status = true;
                        Ro.message = Message.Success;
                        Ro.status_code = 200;
                        Ro.optional = 1;
                    }
                  
                }
                if (Ro.status == true)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, Ro);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound,Ro.message);
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Admin", "GetMonthlyinout");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            #endregion
            return response;
        }

    }
}
