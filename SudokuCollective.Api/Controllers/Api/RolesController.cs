using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers
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

        // POST: api/roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Role>> GetRole(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await rolesService.GetRole(id, fullRecord);

                if (result.Success)
                {
                    return Ok(result.Role);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await rolesService.GetRoles(fullRecord);

                if (result.Success)
                {
                    return Ok(result.Roles);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(
            int id,
            [FromBody] UpdateRoleRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                if (id != request.Id)
                {
                    return BadRequest("Invalid Request: Role Id Incorrect");
                }

                var result = await rolesService.UpdateRole(id, request);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost, Route("Create")]
        public async Task<ActionResult<Role>> PostRole(
            [FromBody] CreateRoleRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await rolesService
                    .CreateRole(request.Name, request.RoleLevel);

                if (result.Success)
                {
                    return CreatedAtAction(
                        "GetRole",
                        new { id = result.Role.Id },
                        result.Role);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(
            int id, [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await rolesService.DeleteRole(id);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
