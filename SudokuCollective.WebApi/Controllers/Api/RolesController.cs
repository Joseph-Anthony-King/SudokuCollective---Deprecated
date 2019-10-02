using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly IRolesService _rolesService;
        private readonly IAppsService _appsService;

        public RolesController(IRolesService rolesService, 
            IAppsService appsService) {
            
            _rolesService = rolesService;
            _appsService = appsService;
        }

        // GET: api/Roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(
            int id, 
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _rolesService.GetRole(id, fullRecord);

                if (result.Success) {

                    return Ok(result.Role);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _rolesService.GetRoles(fullRecord);

                if (result.Success) {

                    return Ok (result.Roles);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(
            int id,
            [FromBody] UpdateRoleRO updateRoleRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateRoleRO.License, 
                updateRoleRO.RequestorId)) {

                if (id != updateRoleRO.Id) {

                    return BadRequest("Invalid Request: Role Id Incorrect");
                }
                
                var result = await _rolesService.UpdateRole(id, updateRoleRO);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/Roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(
            [FromBody] CreateRoleRO createRoleRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                createRoleRO.License, 
                createRoleRO.RequestorId)) {
            
                var result = await _rolesService
                    .CreateRole(createRoleRO.Name, createRoleRO.RoleLevel);

                if (result.Success) {
                    
                    return CreatedAtAction(
                        "GetRole", 
                        new { id = result.Role.Id }, 
                        result.Role);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(
            int id, [FromBody] BaseRequestRO baseRequestRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _rolesService.DeleteRole(id);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
