//using EMP.Models.Master;
//using System;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;

//namespace EMP.Controllers
//{
//    public class ScreenshotController : ApiController
//    {
//        private readonly Dapperr _dapper = new Dapperr();
//        private readonly LogErrors _logErrors = new LogErrors();

//       // [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
//        [HttpPost]
//        public async Task<HttpResponseMessage> GetScreenShots(GetModels obj)
//        {
//            HttpResponseMessage response = null;

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var query = @"SELECT * FROM ScreenShot B WITH(NOLOCK) 
//                                  WHERE B.OrganizationId = @OrganizationId 
//                                  AND B.ServerDate BETWEEN @CDateStart AND @CDateEnd 
//                                  AND B.UserId = @UserId";

//                    var parameters = new
//                    {
//                        OrganizationId = obj.OrganizationId,
//                       // CDateStart = obj.CDate.Date,
//                        CDateEnd = obj.CDate.Date.AddDays(1).AddTicks(-1),
//                        UserId = obj.UserId
//                    };

//                   // var result = await _dapper.GetAllAsync<UserScreenShotModel>(query, parameters);

//                    if (result.Any())
//                    {
//                        response = Request.CreateResponse(HttpStatusCode.OK, result);
//                    }
//                    else
//                    {
//                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data Found");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logErrors.Writelog(ex, "ScreenshotController", "GetScreenShots");
//                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                _logErrors.WriteDirectLog("ScreenshotController", "GetScreenShots: Model State is Not Valid");
//                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
//            }

//            return response;
//        }
//    }
//}
