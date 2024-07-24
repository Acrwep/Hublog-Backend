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
        public HttpResponseMessage GetUserScreenShots(int userId, int organizationId)
        {
            try
            {
                string connectionString = GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var screenshots = connection.Query<UserScreenShotModels>(
                        "SELECT * FROM UserScreenShots WHERE UserId = @UserId AND OrganizationId = @OrganizationId",
                        new { UserId = userId, OrganizationId = organizationId }).ToList();

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
