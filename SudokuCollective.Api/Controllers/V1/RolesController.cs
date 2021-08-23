using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;
using System;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService rolesService;
        private readonly IAppsService appsService;

        public RolesController(
            IRolesService rolesServ,
            IAppsService appsServ)
        {
            rolesService = rolesServ;
            appsService = appsServ;
        }

        // POST: api/roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Role>> Post(
            [FromBody] CreateRoleRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await rolesService
                        .Create(request.Name, request.RoleLevel);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode201(result.Message);

                        return StatusCode((int)HttpStatusCode.Created, result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // PUT: api/roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRoleRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    if (id != request.Id)
                    {
                        return BadRequest(ControllerMessages.IdIncorrectMessage);
                    }

                    var result = await rolesService.Update(id, request);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // DELETE: api/roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(
            int id, [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await rolesService.Delete(id);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // GET: api/roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(
            int id)
        {
            try
            {
                var result = await rolesService.Get(id);

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // GET: api/roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                var result = await rolesService.GetRoles();

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
