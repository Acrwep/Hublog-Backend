using EMP.Models.Master;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EMP.Controllers
{
    public class RoleController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();

        [HttpGet]
        public HttpResponseMessage GetRoleAll()
        {
            HttpResponseMessage response = null;

            try
            {
                var result = _dapper.GetAll<Role>("SELECT * FROM Role WITH (NOLOCK)");

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
                _logErrors.Writelog(ex, "Role", "GetRoleAll");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetRoleById([FromUri] int OrganizationId)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var query = "SELECT * FROM Role WHERE OrganizationId = @OrganizationId AND Active = 1";
                    var result = _dapper.GetAll<Role>(query, new { OrganizationId = OrganizationId });

                    if (result.Count != 0)
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
                    _logErrors.Writelog(ex, "Role", "GetRoleById");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Role", "GetRoleById - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertRole(Role role)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = "INSERT INTO Role (Name, AccessLevel, Description, Admin, URLS, ScreenShot, LiveStream, OrganizationId) VALUES (@Name, @AccessLevel, @Description, @Admin, @URLS, @ScreenShot, @LiveStream, @OrganizationId)";

                    var result = await _dapper.ExecuteAsync(query, role);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, role);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create role");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Role", "InsertRole");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Role", "InsertRole - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateRole(Role role)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = @"UPDATE Role SET Name = @Name, AccessLevel = @AccessLevel, Description = @Description, Admin = @Admin, URLS = @URLS, ScreenShot = @ScreenShot, LiveStream = @LiveStream, OrganizationId = @OrganizationId WHERE Id = @Id";

                    var result = await _dapper.ExecuteAsync(query, role);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, role);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Role not found");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Role", "UpdateRole");
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error updating role");
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Role", "UpdateRole - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteRole(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                string query = "DELETE FROM Role WHERE Id = @Id";

                var result = await _dapper.ExecuteAsync(query, new { Id = id });

                if (result > 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, $"Role with Id {id} deleted successfully");
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Role with Id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logErrors.Writelog(ex, "Role", "DeleteRole");
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error deleting role");
            }

            return response;
        }
    }
}
