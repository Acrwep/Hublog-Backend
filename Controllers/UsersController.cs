using Dapper;
using System.Data;
using System.Data.SqlClient;
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

using EMP.Models; // Adjust the namespace as per your project structure



namespace EMP.Controllers
{
    public class UsersController : ApiController
    {

        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();

        // Ensure this is imported for SQL Server usage

        //[Authorize(Roles = "EMPLOYEE")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertAttendance(List<UserAttendanceModel> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logErrors.WriteDirectLog("InsertAttendance", "Model State is Not Valid");
                    return BadRequest(ModelState);
                }

                foreach (var attendanceModel in model)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", attendanceModel.UserId);
                    parameters.Add("@OrganizationId", attendanceModel.OrganizationId);
                    parameters.Add("@AttendanceDate", attendanceModel.AttendanceDate);
                    parameters.Add("@Start_Time", attendanceModel.Start_Time);
                    parameters.Add("@End_Time", attendanceModel.End_Time);
                    parameters.Add("@Total_Time", attendanceModel.Total_Time);
                    parameters.Add("@Late_Time", attendanceModel.Late_Time);
                    parameters.Add("@Status", attendanceModel.Status);

                    // Execute the stored procedure using Dapper
                    var result = await _dapper.ExecuteAsync("SP_InsertAttendance", parameters, CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                       // return NotFound(new { Message = "Error: Insertion failed for UserId " + attendanceModel.UserId });


                    }
                }

                // Assuming all entries were processed successfully
                return Ok("InsertAttendance" + Message.CreateSuccess);
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "InsertAttendance");
                return InternalServerError(ex);
            }
        }
        // [Authorize(Roles = "EMPLOYEE")]
        //[HttpPost]
        //public HttpResponseMessage InsertAttendance(List<UserAttendanceModel> model)
        //{
        //    Thread.CurrentPrincipal = HttpContext.Current.User;
        //    HttpResponseMessage response = null;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string details = JsonConvert.SerializeObject(model);
        //            details = details.Replace("\"null\"", "\"\"");
        //            details = details.Replace("null", "\"\"");
        //            details = details.Replace("'", "");
        //            var result = Task.FromResult(_dapper.Get<ResultModel>("Exec [SP_InsertAttendance] '" + details + "'"));
        //            if (result.IsCompleted)
        //            {
        //                if (result.Result.Result == 1)
        //                {
        //                    response = Request.CreateResponse(HttpStatusCode.OK, "InsertAttendance" + Message.CreateSuccess);
        //                }
        //                else
        //                {
        //                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error"+ result.Result.Msg);
        //                }
        //            }
        //            else
        //            {
        //                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error" + result.Result.Msg);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logErrors.Writelog(ex, "Users", "InsertAttendance");
        //            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //        }
        //    }
        //    else
        //    {
        //        _logErrors.WriteDirectLog("InsertAttendance", "UserLogin" + "Model State is Not Valid");
        //        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
        //    }
        //    return response;
        //}

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

        #region commented old GetAvailableBreak
        //[Authorize(Roles = "EMPLOYEE")]
        //[HttpPost]
        //public HttpResponseMessage GetAvailableBreak(GetModels obj)
        //{
        //    Thread.CurrentPrincipal = HttpContext.Current.User;
        //    HttpResponseMessage response = null;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var result = Task.FromResult(_dapper.GetAll<BreakMaster>("select * from BreakMaster B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.Active=1 and B.id not in(select distinct(BreakEntryId) from BreakEntry BE where BE.BreakDate='" + obj.CDate + "' and  BE.UserId='" + obj.UserId + "' and  BE.OrganizationId='" + obj.OrganizationId + "') ").ToList());
        //            if (result.IsCompleted)
        //            {
        //                if (result.Result.Count != 0)
        //                {
        //                    response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
        //                }
        //                else
        //                {
        //                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
        //                }
        //            }
        //            else
        //            {
        //                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logErrors.Writelog(ex, "Users", "GetAvailableBreak");
        //            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //        }
        //    }
        //    else
        //    {
        //        _logErrors.WriteDirectLog("Users", "GetAvailableBreak" + "Model State is Not Valid");
        //        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
        //    }
        //    return response;
        //}
        #endregion

        #region New GetAvailableBreak
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
                    var result = Task.FromResult(_dapper.GetAll<BreakMaster>(
                        "SELECT * FROM BreakMaster B WITH(NOLOCK) " +
                        "WHERE B.OrganizationId = @OrganizationId " +
                        "AND B.Active = 1 " +
                        "AND B.Id NOT IN (SELECT DISTINCT BE.Id " +
                        "FROM BreakEntry BE " +
                        "WHERE CAST(BE.Start_Time AS DATE) = @CDate " +
                        "AND BE.UserId = @UserId " +
                        "AND BE.OrganizationId = @OrganizationId)"
                    , new
                    {
                        OrganizationId = obj.OrganizationId,
                        CDate = obj.CDate,
                        UserId = obj.UserId
                    }).ToList());

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
        #endregion


        [HttpGet]
        public HttpResponseMessage GetAllUsers()
        {
            HttpResponseMessage response = null;

            try
            {
                var result = _dapper.GetAll<Users>("SELECT * FROM Users WITH (NOLOCK)");

                if (result != null && result.Any())
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "GetAllUsers");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetUsersByOrganizationId([FromUri] int OrganizationId)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var query = "SELECT * FROM Users WHERE OrganizationId = @OrganizationId AND Active = 1";
                    var result = _dapper.GetAll<Users>(query, new { OrganizationId = OrganizationId });

                    if (result.Any())
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "GetUsersByOrganizationId");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "GetUsersByOrganizationId - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertUser(Users user)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = @"
                INSERT INTO Users (First_Name, Last_Name, Email, DOB, DOJ, Phone, UsersName, Password, 
Gender, OrganizationId, RoleId, DesignationId, TeamId, Active, EmployeeID) 
                VALUES (@First_Name, @Last_Name, @Email, @DOB, @DOJ, @Phone, @UsersName, @Password,
@Gender, @OrganizationId, @RoleId, @DesignationId, @TeamId, @Active, @EmployeeID)";

                    user.Active = true;

                    var result = await _dapper.ExecuteAsync(query, user);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, user);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create user");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "InsertUser");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "InsertUser - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateUser(Users user)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = @"
                UPDATE Users 
                SET First_Name = @First_Name, Last_Name = @Last_Name, Email = @Email, DOB = @DOB, DOJ = @DOJ, Phone = @Phone, 
                    UsersName = @UsersName, Password = @Password, Gender = @Gender, OrganizationId = @OrganizationId, 
                    RoleId = @RoleId, DesignationId = @DesignationId, TeamId = @TeamId, Active = @Active, EmployeeID = @EmployeeID 
                WHERE Id = @Id";

                    var result = await _dapper.ExecuteAsync(query, user);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "UpdateUser");
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error updating user");
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "UpdateUser - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }


        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                string query = "DELETE FROM Users WHERE Id = @Id";

                var result = await _dapper.ExecuteAsync(query, new { Id = id });

                if (result > 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, $"User with Id {id} deleted successfully");
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, $"User with Id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "DeleteUser");
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error deleting user");
            }

            return response;
        }
    
}
}
