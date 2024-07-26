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
        [Authorize(Roles = "SUPER_ADMIN,ADMIN")]
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
                      ,uss.[FilePath]
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
        [Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpGet]
        [Route("api/Screenshot/GetAvailableIntervals")]
        public HttpResponseMessage GetAvailableIntervals()
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = "SELECT " +
                        "           SI.IntervalInMilliseconds," +
                        "           SI.SelectedIntervalId," +
                        "           SI.Description" +
                        "       FROM ScreenshotIntervals SI" +
                        "       INNER JOIN ScreenshotSettings SS ON SI.Id = ";
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

    }
}
