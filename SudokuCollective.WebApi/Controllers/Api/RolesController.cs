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

            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _rolesService.GetRole(id, fullRecord);

                if (result.Result) {

                    return Ok(result.Role);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _rolesService.GetRoles(fullRecord);

                if (result.Result) {

                    return Ok (result.Roles);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // PUT: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(
            int id,
            [FromBody] UpdateRoleRO updateRoleRO) {
            
            if (_appsService.ValidLicense(updateRoleRO.License)) {

                if (id != updateRoleRO.Id) {

                    return BadRequest();
                }
                
                var result = await _rolesService.UpdateRole(id, updateRoleRO);
                
                if (result) {

                    return Ok();

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // POST: api/Roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(
            [FromBody] CreateRoleRO createRoleRO) {
            
            if (_appsService.ValidLicense(createRoleRO.License)) {
            
                var result = await _rolesService
                    .CreateRole(createRoleRO.Name, createRoleRO.RoleLevel);

                if (result.Result) {
                    
                    return CreatedAtAction(
                        "GetRole", 
                        new { id = result.Role.Id }, 
                        result.Role);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // DELETE: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(
            int id, [FromBody] BaseRequestRO baseRequestRO) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _rolesService.DeleteRole(id);
                
                if (result) {

                    return Ok();

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }
    }
}
