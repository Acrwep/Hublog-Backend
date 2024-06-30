using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EMP.Models.Master;
using Dapper;

namespace EMP.Controllers
{
    //[Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
    public class TeamController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();

        [HttpGet]
        public HttpResponseMessage GetTeam(int organizationId)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = Task.FromResult(_dapper.GetAll<Team>("SELECT * FROM Team WHERE OrganizationId = @OrganizationId AND Active = 1", new { OrganizationId = organizationId }).ToList());
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
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error fetching data");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "GetTeam");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "GetTeam - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateTeam([FromBody] Team team)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    string query = "INSERT INTO Team (Name, Active, Description, OrganizationId, Parentid) " +
                                   "VALUES (@Name, @Active, @Description, @OrganizationId, @Parentid)";

                    team.Active = true;

                    var result = await _dapper.ExecuteAsync(query, team);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, team);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create team");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "CreateTeam");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "CreateTeam - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateTeam(int id, [FromBody] Team updatedTeam)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    string query = "UPDATE Team SET Name = @Name, Description = @Description, Active = @Active, OrganizationId = @OrganizationId, Parentid = @Parentid WHERE Id = @Id";

                    updatedTeam.Id = id;
                    var result = await _dapper.ExecuteAsync(query, updatedTeam);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, updatedTeam);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not update team");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "UpdateTeam");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "UpdateTeam - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteTeam(int id)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    string query = "DELETE FROM Team WHERE Id = @Id";

                    var result = await _dapper.ExecuteAsync(query, new { Id = id });
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not delete team");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Admin", "DeleteTeam");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Admin", "DeleteTeam - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }
    }
}
