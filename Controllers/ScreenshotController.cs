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

        //USERS,EMPLOYEE
        [Authorize(Roles = "SUPER_ADMIN,ADMIN")]
        [HttpGet]
        [Route("api/Users/GetUserScreenShots")]
        public HttpResponseMessage GetUserScreenShots(int userId, int organizationId, int teamId, DateTime startDate, DateTime endDate)
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var startDateTime = startDate.Date;
                    var endDateTime = endDate.Date.AddDays(1).AddSeconds(-1);

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
                      ,u.[TeamId]
                      ,t.[Name] AS TeamName
                  FROM [EMP2].[dbo].[UserScreenShots] uss
                  JOIN [EMP2].[dbo].[Users] u ON uss.[UserId] = u.[Id]
                  JOIN [EMP2].[dbo].[Team] t ON u.[TeamId] = t.[Id]
                  WHERE uss.[ScreenShotDate] >= @StartDateTime
                    AND uss.[ScreenShotDate] <= @EndDateTime
                    AND uss.[UserId] = @UserId
                    AND uss.[OrganizationId] = @OrganizationId
                    AND u.[TeamId] = @TeamId";

                    var screenshots = connection.Query<UserScreenShotModels>(
                        query,
                        new
                        {
                            UserId = userId,
                            OrganizationId = organizationId,
                            TeamId = teamId,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime
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


        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString;
        }
    }
}
