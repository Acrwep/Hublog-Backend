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
using System.Configuration;
using System.Security.Claims;
using System.Globalization;



namespace EMP.Controllers
{
    public class UsersController : ApiController
    {

        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString;
        }

        #region NEW InsertAttendance

        [Authorize(Roles = "EMPLOYEE")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertAttendance(List<UserAttendanceModel> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logErrors.WriteDirectLog("InsertAttendance", "Model State is Not Valid");

                    // Log details about why the model state is invalid
                    foreach (var state in ModelState)
                    {
                        if (state.Value.Errors.Count > 0)
                        {
                            _logErrors.WriteDirectLog("InsertAttendance", $"Property: {state.Key}, Error: {state.Value.Errors[0].ErrorMessage}");
                        }
                    }

                    return BadRequest(ModelState);
                }

                foreach (var attendanceModel in model)
                {
                    //string formattedAttendanceDate = attendanceModel.AttendanceDate.ToString("yyyy-MM-dd HH:mm:ss");
                    //string formattedStart_Time = attendanceModel.Start_Time?.ToString("yyyy-MM-dd HH:mm:ss");
                    //string formattedEnd_Time = attendanceModel.End_Time?.ToString("yyyy-MM-dd HH:mm:ss");

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", attendanceModel.UserId);
                    parameters.Add("@OrganizationId", attendanceModel.OrganizationId);
                    parameters.Add("@AttendanceDate", attendanceModel.AttendanceDate);
                    parameters.Add("@Start_Time", attendanceModel.Start_Time);
                    parameters.Add("@End_Time", attendanceModel.End_Time);
                    parameters.Add("@Total_Time", null);
                    parameters.Add("@Late_Time", null);
                    parameters.Add("@Status", attendanceModel.Status);

                    var result = await _dapper.ExecuteAsync("SP_InsertAttendance", parameters, CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        // return NotFound(new { Message = "Error: Insertion failed for UserId " + attendanceModel.UserId });
                    }
                }
                return Ok("InsertAttendance" + Message.CreateSuccess);
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "InsertAttendance");
                return InternalServerError(ex);
            }
        }




        //[Authorize(Roles = "EMPLOYEE")]
        //[HttpPost]
        //public async Task<IHttpActionResult> InsertAttendance(List<UserAttendanceModel> model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            _logErrors.WriteDirectLog("InsertAttendance", "Model State is Not Valid");
        //            foreach (var state in ModelState)
        //            {
        //                if (state.Value.Errors.Count > 0)
        //                {
        //                    _logErrors.WriteDirectLog("InsertAttendance", $"Property: {state.Key}, Error: {state.Value.Errors[0].ErrorMessage}");
        //                }
        //            }

        //            return BadRequest(ModelState);
        //        }

        //        foreach (var attendanceModel in model)
        //        {
        //            string formattedAttendanceDate = attendanceModel.AttendanceDate.ToString("yyyy-MM-dd HH:mm:ss");
        //            string formattedStart_Time = attendanceModel.Start_Time?.ToString("yyyy-MM-dd HH:mm:ss");
        //            string formattedEnd_Time = attendanceModel.End_Time?.ToString("yyyy-MM-dd HH:mm:ss");

        //            var parameters = new DynamicParameters();
        //            parameters.Add("@UserId", attendanceModel.UserId);
        //            parameters.Add("@OrganizationId", attendanceModel.OrganizationId);
        //            parameters.Add("@AttendanceDate", formattedAttendanceDate);
        //            parameters.Add("@Start_Time", formattedStart_Time);
        //            parameters.Add("@End_Time", formattedEnd_Time);
        //            parameters.Add("@Total_Time", null);
        //            parameters.Add("@Late_Time", null);
        //            parameters.Add("@Status", attendanceModel.Status);

        //            var result = await _dapper.ExecuteAsync("SP_InsertAttendance", parameters, CommandType.StoredProcedure);

        //            if (result <= 0)
        //            {
        //                // return NotFound(new { Message = "Error: Insertion failed for UserId " + attendanceModel.UserId });
        //            }
        //        }
        //        return Ok("InsertAttendance" + Message.CreateSuccess);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logErrors.Writelog(ex, "Users", "InsertAttendance");
        //        return InternalServerError(ex);
        //    }
        //}
        #endregion

        #region new insertbreak
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

                    var formattedDetails = new List<dynamic>();
                    foreach (var item in model)
                    {
                        formattedDetails.Add(new
                        {
                            item.OrganizationId,
                            item.BreakEntryId,
                            item.Id,
                            item.UserId,
                            BreakDate = item.BreakDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            Start_Time = item.Start_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                            End_Time = item.End_Time?.ToString("yyyy-MM-dd HH:mm:ss"),
                            item.Status
                        });
                    }
                    details = JsonConvert.SerializeObject(formattedDetails);

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
        #endregion

        #region New UploadFile
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
                OId = Convert.ToInt32(Request.Headers.GetValues("OId").First());
                SType = Request.Headers.GetValues("SType").First();
                SDate = Request.Headers.GetValues("SDate").First();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Header parsing error: {ex.Message}");
            }

            try
            {
                DateTime parsedDate;
                if (!DateTime.TryParse(SDate, out parsedDate))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid date format.");
                }
                string formattedDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                    if (pic != null && pic.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var fileExtension = Path.GetExtension(fileName);
                        var imageName = (SType == "ScreenShots") ? fileName.Replace(fileExtension, "") : Guid.NewGuid().ToString();
                        var newFileName = imageName + fileExtension;

                        byte[] imageData;
                        using (var ms = new MemoryStream())
                        {
                            pic.InputStream.CopyTo(ms);
                            imageData = ms.ToArray();
                        }

                        if (imageData.Length == 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Image data is empty.");
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@UserId", UId);
                        parameters.Add("@OrganizationId", OId);
                        parameters.Add("@ScreenShotDate", formattedDate);
                        parameters.Add("@FileName", newFileName);
                        parameters.Add("@ImageData", imageData);

                        string connectionString = GetConnectionString();
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            connection.Execute("SP_InsertScreenShot", parameters, commandType: CommandType.StoredProcedure);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, "Upload successful.");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "No file uploaded.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No files found.");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "User", "UploadFile");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region GetUserAttendanceDetails
        [HttpGet]
        public HttpResponseMessage GetUserAttendanceDetails([FromUri] int userId, [FromUri] DateTime? startDate = null, [FromUri] DateTime? endDate = null)
        {
            HttpResponseMessage response = null;
            try
            {
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    DateTime today = DateTime.Today;
                    int diff = today.DayOfWeek - DayOfWeek.Monday;
                    DateTime startOfWeek = today.AddDays(-diff).Date;
                    DateTime endOfWeek = startOfWeek.AddDays(6);

                    startDate = startOfWeek;
                    endDate = endOfWeek;
                }
                var queryAttendance = @"
            SELECT 
                U.First_Name AS FirstName, 
                U.Email, 
                U.EmployeeID AS EmployeeId, 
                U.Active, 
                A.AttendanceDate, 
                A.Start_Time, 
                A.End_Time, 
                A.Total_Time, 
                A.Late_Time, 
                A.Status 
            FROM Users U
                INNER JOIN Attendance A ON U.Id = A.UserId
                WHERE U.Id = @UserId
                  AND A.AttendanceDate BETWEEN @StartDate AND @EndDate";

                var parameters = new { UserId = userId, StartDate = startDate.Value, EndDate = endDate.Value };

                var attendanceRecords = Task.FromResult(_dapper.GetAll<UserAttendanceDetailModel>(queryAttendance, parameters).ToList());

                if (attendanceRecords.IsCompleted)
                {
                    var records = attendanceRecords.Result;

                    var allDates = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                        .Select(offset => startDate.Value.AddDays(offset))
                        .ToList();

                    int daysPresent = 0;
                    int daysAbsent = 0;

                    foreach (var date in allDates)
                    {
                        if (date.DayOfWeek == DayOfWeek.Sunday) continue;

                        var record = records.FirstOrDefault(r => r.AttendanceDate.Date == date.Date);

                        if (record != null)
                        {
                            daysPresent++;
                        }
                        else
                        {
                            daysAbsent++;
                        }
                    }

                    var summary = new AttendanceSummaryModel
                    {
                        DaysPresent = daysPresent,
                        DaysLeave = daysAbsent
                    };

                    var responseModel = new
                    {
                        AttendanceDetails = records,
                        AttendanceSummary = summary
                    };

                    response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "GetUserAttendanceDetails");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        #endregion

        #region  GetUsersByTeamId
        [HttpGet]
        public HttpResponseMessage GetUsersByTeamId([FromUri] int TeamId)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var query = @"
                SELECT 
                    u.[Id],
                    u.[First_Name],
                    u.[Last_Name],
                    u.[Email],
                    u.[DOB],
                    u.[DOJ],
                    u.[Phone],
                    u.[UsersName],
                    u.[Password],
                    u.[Gender],
                    u.[OrganizationId],
                    u.[RoleId],
                    u.[DesignationId],
                    u.[TeamId],
                    u.[Active],
                    u.[EmployeeID],
                    t.[Name] AS TeamName
                FROM 
                    [dbo].[Users] u
                INNER JOIN 
                    [dbo].[Team] t
                ON 
                    u.[TeamId] = t.[Id]
                WHERE 
                    u.[TeamId] = @TeamId
                    ";

                    var result = _dapper.GetAll<dynamic>(query, new { TeamId = TeamId });

                    var teamData = result.FirstOrDefault();
                    if (teamData == null)
                    {
                        var responseData = new
                        {
                            Team = new
                            {
                                TeamId = TeamId,
                                TeamName = string.Empty,
                                Users = new List<object>()
                            }
                        };

                        response = Request.CreateResponse(HttpStatusCode.OK, responseData);
                    }
                    else
                    {
                        int teamId = teamData.TeamId;
                        string teamName = teamData.TeamName;

                        var responseData = new
                        {
                            Team = new
                            {
                                TeamId = teamId,
                                TeamName = teamName,
                                Users = result.Select(u => new
                                {
                                    UserId = (int)u.Id,
                                    FirstName = (string)u.First_Name,
                                    LastName = (string)u.Last_Name,
                                    Email = (string)u.Email,
                                    DOB = (DateTime)u.DOB,
                                    DOJ = (DateTime)u.DOJ,
                                    Phone = (string)u.Phone,
                                    UsersName = (string)u.UsersName,
                                    Password = (string)u.Password,
                                    Gender = (string)u.Gender,
                                    OrganizationId = (int)u.OrganizationId,
                                    RoleId = (int)u.RoleId,
                                    DesignationId = (int)u.DesignationId,
                                    TeamId = (int)u.TeamId,
                                    Active = (bool)u.Active,
                                    EmployeeID = (string)u.EmployeeID
                                }).ToList()
                            }
                        };

                        response = Request.CreateResponse(HttpStatusCode.OK, responseData);
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Users", "GetUsersByTeamId");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Users", "GetUsersByTeamId - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        #endregion

        #region GetUsersByOrganizationId
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
        #endregion

        #region  User CRUD OPS

        [HttpGet]
        public HttpResponseMessage GetAllUsers()
        {
            HttpResponseMessage response = null;

            try
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var loggedInUserEmail = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var result = _dapper.GetAll<Users>("SELECT * FROM Users WITH (NOLOCK)");

                if (result != null && result.Any())
                {
                    var sortedResult = result.OrderByDescending(u => u.Email == loggedInUserEmail).ToList();
                    response = Request.CreateResponse(HttpStatusCode.OK, sortedResult);
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
        #endregion

        #region GetAvailableBreak

        [HttpPost]
        public HttpResponseMessage GetAvailableBreak(GetModels obj)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string sqlQuery = @"
                SELECT * 
                FROM BreakMaster B WITH (NOLOCK)
                WHERE B.OrganizationId = @OrganizationId
                  AND B.Active = 1
                  AND B.Id NOT IN (
                      SELECT DISTINCT Id
                      FROM BreakEntry BE
                      WHERE BE.Start_Time = @CDate
                        AND BE.UserId = @UserId
                        AND BE.OrganizationId = @OrganizationId
                  )";

                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", obj.OrganizationId);
                    parameters.Add("@CDate", obj.CDate); 
                    parameters.Add("@UserId", obj.UserId);

                    // Execute query
                    var result = Task.FromResult(_dapper.GetAll<BreakMaster>(sqlQuery, parameters).ToList());

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
                _logErrors.WriteDirectLog("Users", "GetAvailableBreak: Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }



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
        //            //var result = Task.FromResult(_dapper.GetAll<BreakMaster>("select * from BreakMaster B with(Nolock) where B.OrganizationId=" + obj.OrganizationId + " and B.Active=1 and B.id not in(select distinct(Id) from BreakEntry BE where BE.BreakDate='" + obj.CDate + "' and  BE.UserId='" + obj.UserId + "' and  BE.OrganizationId='" + obj.OrganizationId + "') ").ToList());
        //            var result = Task.FromResult(_dapper.GetAll<BreakMaster>(
        //                        "select * from BreakMaster B with(Nolock) where B.OrganizationId=" + obj.OrganizationId +
        //                        " and B.Active=1 and B.id not in(select distinct(Id) from BreakEntry BE where BE.Start_Time='" + obj.CDate +
        //                        "' and BE.UserId='" + obj.UserId + "' and BE.OrganizationId='" + obj.OrganizationId + "')").ToList());

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

        #region GetUserBreakRecordDetails
        [HttpGet]
        public HttpResponseMessage GetUserBreakRecordDetails([FromUri] int userId, [FromUri] DateTime? startDate = null, [FromUri] DateTime? endDate = null)
        {
            HttpResponseMessage response = null;
            try
            {
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    DateTime today = DateTime.Today;
                    int diff = today.DayOfWeek - DayOfWeek.Monday;
                    DateTime startOfWeek = today.AddDays(-diff).Date;
                    DateTime endOfWeek = startOfWeek.AddDays(6);

                    startDate = startOfWeek;
                    endDate = endOfWeek;
                }

                DateTime endDateTime = endDate.Value.Date.AddDays(1);

                var query = @"
                    SELECT 
                        BE.UserId, 
                        BE.OrganizationId, 
                        BE.BreakDate, 
                        BE.Start_Time, 
                        BE.End_Time, 
                        BE.BreakEntryId, 
                        BE.Status,
                        BE.BreakDuration,
                        BM.Name as BreakType, 
                        BM.Active, 
                        BM.Max_Break_Time, 
                        U.First_Name as firstName, 
                        U.Email
                    FROM BreakEntry BE 
                    INNER JOIN BreakMaster BM ON BE.BreakEntryId = BM.Id
                    INNER JOIN Users U ON U.Id = BE.UserId
                    WHERE BE.UserId = @UserId
                    AND (
                        (BE.BreakDate BETWEEN @StartDate AND @EndDate)
                        OR (BE.Start_Time <= @EndDateTime AND BE.End_Time >= @StartDate)
                    )
                    ";

                var parameters = new
                {
                    UserId = userId,
                    StartDate = startDate.Value.Date,
                    EndDate = endDate.Value.Date.AddDays(1).AddTicks(-1), 
                    EndDateTime = endDateTime
                };

                var result = Task.FromResult(_dapper.GetAll<UserBreakRecordModel>(query, parameters).ToList());

                if (result.IsCompleted)
                {
                    if (result.Result.Count != 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, new List<UserBreakRecordModel>());
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "Error");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Users", "GetUserBreakRecordDetails");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        #endregion

        #region GetBreakMasterById
        [HttpGet]
        public async Task<HttpResponseMessage> GetBreakMasterById(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var query = "SELECT * FROM BreakMaster WHERE Id = @Id";
                var parameters = new { Id = id };
                var result = await _dapper.GetAsync<BreakMaster>(query, parameters);

                if (result != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "BreakMaster record not found");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "BreakMaster", "GetBreakMasterById");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }
        #endregion
    }
}
