using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.RoleRequests;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService) {
            
            _rolesService = rolesService;
        }

        // GET: api/Roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id) {

            var role = await _rolesService.GetRole(id);

            if (string.IsNullOrEmpty(role.Value.Name)) {

                return BadRequest();

            } else {

                return role;
            }
        }

        // GET: api/Roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles() {

            return await _rolesService.GeRoles();
        }

        // PUT: api/Roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role) {

            if (id != role.Id) {

                return BadRequest();
            }
            
            await _rolesService.UpdateRole(id, role);

            return NoContent();
        }

        // POST: api/Roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole([FromBody] CreateRoleRO createRoleRO) {
            
            var role = await _rolesService
                .CreateRole(createRoleRO.Name, createRoleRO.RoleLevel);

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id) {

            var role = await _rolesService.DeleteRole(id);

            return role;
        }
    }
}
