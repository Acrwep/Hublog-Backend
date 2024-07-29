using Dapper;
using EMP.Models.Master;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EMP.Controllers
{
    public class ScreenshotController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString;
        }

        //USERS,EMPLOYEE
        #region  GetUserScreenShots
        //[Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpGet]
        [Route("api/Users/GetUserScreenShots")]
        public HttpResponseMessage GetUserScreenShots(int userId, int organizationId, DateTime date)
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = @"
                SELECT uss.[Id]
                      ,uss.[UserId]
                      ,uss.[OrganizationId]
                      ,uss.[ScreenShotDate]
                      ,uss.[FileName]
                      ,uss.[ImageData]
                      ,u.[First_Name]
                      ,u.[Last_Name]
                      ,u.[Email]
                  FROM [dbo].[UserScreenShots] uss
                  JOIN [dbo].[Users] u ON uss.[UserId] = u.[Id]
                  JOIN [dbo].[Team] t ON u.[TeamId] = t.[Id]
                  WHERE CAST(uss.[ScreenShotDate] AS DATE) = @Date
                    AND uss.[UserId] = @UserId
                    AND uss.[OrganizationId] = @OrganizationId";

                    var screenshots = connection.Query<UserScreenShotModels>(
                        query,
                        new
                        {
                            UserId = userId,
                            OrganizationId = organizationId,
                            Date = date
                        }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, screenshots);
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "User", "GetUserScreenShots");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.InnerException?.Message ?? ex.Message);
            }
        }
        #endregion

        #region GetAvailableIntervals
        //[Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpGet]
        [Route("api/Screenshot/GetAvailableIntervals")]
        public HttpResponseMessage GetAvailableIntervals()
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = @"SELECT
                                    SI.IntervalInMilliseconds,
                                    SI.Description,
                                    SS.SelectedIntervalId
                                FROM ScreenshotIntervals SI
                                INNER JOIN ScreenshotSettings SS ON SI.Id = SS.SelectedIntervalId";
                    var intervals = connection.Query<ScreenshotIntervalModel>(query).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, intervals);
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Screenshot", "GetAvailableIntervals");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }
        }
        #endregion

        #region SetSelectedInterval
        //[Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpPost]
        [Route("api/Screenshot/SetSelectedInterval")]
        public HttpResponseMessage SetSelectedInterval([FromBody] int intervalId)
        {
            try
            {
                if (intervalId <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid interval ID.");
                }

                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var query = @"UPDATE ScreenshotSettings SET SelectedIntervalId = @IntervalId WHERE Id = 1";
                        var rowsAffected = connection.Execute(query, new { IntervalId = intervalId }, transaction);

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Selected interval updated successfully.");
                        }
                        else
                        {
                            transaction.Rollback();
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Setting not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Screenshot", "SetSelectedInterval");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion



        #region GetSelectedInterval
        [HttpGet]
        [Route("api/Screenshot/GetSelectedInterval")]
        public HttpResponseMessage GetSelectedInterval()
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = @"SELECT SI.IntervalInMilliseconds, SS.Id, SS.SelectedIntervalId
                                    FROM ScreenshotIntervals SI
                                    INNER JOIN ScreenshotSettings SS ON SI.Id = SS.SelectedIntervalId";
                    var intervals = connection.Query<ScreenshotIntervalModel>(query).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, intervals);
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Screenshot", "GetSelectedInterval");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }
        }
        #endregion

        [HttpDelete]
        [Route("api/screenshots/cleanup")]
        public async Task<HttpResponseMessage> DeleteOldScreenshots()
        {
            HttpResponseMessage response = null;

            try
            {
                string query = @"
            DELETE FROM UserScreenShots
            WHERE ScreenShotDate < DATEADD(day, -1, GETDATE())";

                var result = await _dapper.ExecuteAsync(query);

                if (result > 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, "Old screenshots deleted successfully");
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No old screenshots found to delete");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "UserScreenShots", "DeleteOldScreenshots");
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error deleting old screenshots");
            }

            return response;
        }
    }
}

