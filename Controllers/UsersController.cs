using EMP.Models.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EMP.Controllers
{
    public class UsersController : ApiController
    {

        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();



        [Authorize(Roles = "EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage InsertAttendance(List<UserAttendanceModel> model)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    string details = JsonConvert.SerializeObject(model);
                    details = details.Replace("\"null\"", "\"\"");
                    details = details.Replace("null", "\"\"");
                    details = details.Replace("'", "");
                    var result = Task.FromResult(_dapper.Get<ResultModel>("Exec [SP_Attendance] '" + details + "'"));
                    if (result.IsCompleted)
                    {
                        if (result.Result.Result == 1)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, "InsertAttendance" + Message.CreateSuccess);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error"+ result.Result.Msg);
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error" + result.Result.Msg);
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "InsertAttendance");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("InsertAttendance", "UserLogin" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [Authorize(Roles = "EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage InsertBreak(List<UserBreakModel> model)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    string details = JsonConvert.SerializeObject(model);
                    details = details.Replace("\"null\"", "\"\"");
                    details = details.Replace("null", "\"\"");
                    details = details.Replace("'", "");
                    var result = Task.FromResult(_dapper.Get<ResultModel>("Exec [SP_BreakEntry] '" + details + "'"));
                    if (result.IsCompleted)
                    {
                        if (result.Result.Result == 1)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, "InsertBreak" + Message.CreateSuccess);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error" + result.Result.Msg);
                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error" + result.Result.Msg);
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "InsertBreak");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("InsertAttendance", "UserLogin" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }


        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            Int32 UId = 0;
            Int32 OId = 0;
            string SType = "";
            string SDate = "";
            try
            {
                UId = Convert.ToInt32(Request.Headers.GetValues("UId").First());
            }
            catch
            {

            }
            try
            {
                OId = Convert.ToInt32(Request.Headers.GetValues("OId").First());
            }
            catch
            {

            }
            try
            {
                SType = Request.Headers.GetValues("SType").First();
            }
            catch
            {

            }
            try
            {
                SDate = Request.Headers.GetValues("SDate").First();
            }
            catch
            {

            }
            try
            {
                string path = string.Empty;
                string _imgname = "";
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                    if (pic.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var _ext = Path.GetExtension(pic.FileName);
                        if (SType == "ScreenShots")
                        {
                            _imgname = fileName.Replace(_ext, "");
                        }
                        else
                        {
                            _imgname = Guid.NewGuid().ToString();
                        }
                        //string path1 = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        string path1 = System.Web.Hosting.HostingEnvironment.MapPath("~/ScreenShots/" + OId + "/" + UId + "/");
                        if (!(Directory.Exists(path1)))
                        {
                            Directory.CreateDirectory(path1);
                        }
                        var _comPath = HttpContext.Current.Server.MapPath("/ScreenShots/" + OId + "/" + UId + "/") + _imgname + _ext;
                        _imgname = _imgname + _ext;
                        path = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ("/ScreenShots/" + OId + "/" + UId + "/") + _imgname;
                        Stream strm = pic.InputStream;
                        objfun.Compressimage(strm, _comPath, pic.FileName);
                        if (SType == "ScreenShots")
                        {
                            List<UserScreenShotModel> obj = new List<UserScreenShotModel>();
                            obj.Add(new UserScreenShotModel()
                            {
                                UserId = UId,
                                OrganizationId = OId,
                                ScreenShotDate = SDate,
                                FileName = _imgname,
                                FilePath = path,
                            }); ;
                            string details = JsonConvert.SerializeObject(obj);
                            details = details.Replace("\"null\"", "\"\"");
                            details = details.Replace("null", "\"\"");
                            details = details.Replace("'", "");
                            _logErrors.WriteDirectLog("Users", "Exec [SP_ScreenShot]" + details);
                            var result = Task.FromResult(_dapper.Get<ResultModel>("Exec [SP_ScreenShot] '" + details + "'"));
                            if (result.IsCompleted)
                            {
                                //_logErrors.WriteDirectLog("Users", "UploadFile" + result.Result.Msg);
                            }
                            else
                            {
                                _logErrors.WriteDirectLog("Users", "UploadFile" + result.Result.Msg);
                            }
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "User", "UploadFile");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

        }


        [Authorize(Roles = "EMPLOYEE")]
        [HttpPost]
        public HttpResponseMessage GetAvailableBreak(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<BreakMaster>("select * from BreakMaster B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.Active=1 and B.id not in(select distinct(BreakEntryId) from BreakEntry BE where BE.BreakDate='" + obj.CDate + "' and  BE.UserId='" + obj.UserId + "' and  BE.OrganizationId='" + obj.OrganizationId + "') ").ToList());
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
                    _logErrors.Writelog(ex, "Users", "GetAvailableBreak");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "GetAvailableBreak" + "Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }
    }
}
