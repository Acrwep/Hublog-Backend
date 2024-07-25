using EMP.Models.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EMP.Controllers
{
    public class DesignationController : ApiController
    {
        private readonly Dapperr _dapper = new Dapperr();
        private readonly LogErrors _logErrors = new LogErrors();
        CommonFunctiton objfun = new CommonFunctiton();


        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetDesignationAll()
        {
            HttpResponseMessage response = null;

            try
            {
                var result = _dapper.GetAll<Designation>("SELECT * FROM Designation WITH (NOLOCK)");

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
                _logErrors.Writelog(ex, "Designation", "GetDesignation");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        [Authorize(Roles = "SUPER_ADMIN,ADMIN,USERS,EMPLOYEE")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetDesignationById([FromUri] int OrganizationId)
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var query = "SELECT * FROM Designation B WITH (NOLOCK) WHERE B.OrganizationId = @OrganizationId AND B.Active = 1";
                    var result = _dapper.GetAll<Designation>(query, new { OrganizationId = OrganizationId });

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
                    _logErrors.Writelog(ex, "Designation", "GetDesignationById");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Designation", "GetDesignationById - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertDesignation(Designation designation)
        {
            //Thread.CurrentPrincipal = HttpContext.Current.User;
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = "INSERT INTO Designation (Name, Active, Description, Created_date, OrganizationId) VALUES (@Name, @Active, @Description, @Created_date, @OrganizationId)";

                    designation.Active = true;

                    var result = await _dapper.ExecuteAsync(query, designation);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, designation);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create designation");
                    }
                }
                catch (Exception ex)
                {
                    _logErrors.Writelog(ex, "Designation", "InsertDesignation");
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Designation", "CreateDesignation - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }
            return response;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateDesignation(Designation designation)
        {
            HttpResponseMessage response = null;

            if (ModelState.IsValid)
            {
                try
                {
                    string query = @"UPDATE Designation SET Name = @Name, Active = @Active, Description = @Description, Created_date = @Created_date, OrganizationId = @OrganizationId WHERE Id = @Id";

                    // Assuming Id is a required property for update
                    var result = await _dapper.ExecuteAsync(query, designation);
                    if (result > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, designation);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Designation not found");
                    }
                }
                catch (Exception ex)
                {
                    // _logErrors.WriteLog(ex, "Designation", "UpdateDesignation");
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error updating designation");
                }
            }
            else
            {
                _logErrors.WriteDirectLog("Designation", "UpdateDesignation - Model State is Not Valid");
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State is Not Valid");
            }

            return response;
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteDesignation(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                string query = "DELETE FROM Designation WHERE Id = @Id";

                // Execute the deletion query
                var result = await _dapper.ExecuteAsync(query, new { Id = id });

                if (result > 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, $"Designation with Id {id} deleted successfully");
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Designation with Id {id} not found");
                }
            }
            catch (Exception ex)
            {
                // _logErrors.WriteLog(ex, "Admin", "DeleteDesignation");
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error deleting designation");
            }

            return response;
        }


    }
}
