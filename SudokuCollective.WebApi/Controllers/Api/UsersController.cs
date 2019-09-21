using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUsersService _userService;

        public UsersController(IUsersService userService) {

            _userService = userService;
        }

        // GET: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id, [FromQuery] bool fullRecord = true) {

            var user = await _userService.GetUser(id, fullRecord);

            if (!_userService.IsUserValid(user.Value)) {

                return NotFound();

            } else {

                return user;
            }
        }

        // GET: api/Users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromQuery] bool fullRecord = true) {

            return await _userService.GetUsers(fullRecord);
        }

        // PUT: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRO updateUserRO) {
            
            var user = await _userService.UpdateUser(id, updateUserRO);

            if (!_userService.IsUserValid(user.Value)) {

                return NotFound();

            } else if (user.Value.Email.Equals(updateUserRO.Email) 
                && user.Value.Id == 0) {

                return BadRequest("Email is not unique");

            } else {

                return NoContent();
            }
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            
            var user = await _userService.DeleteUser(id);

            if (!_userService.IsUserValid(user)) {

                return NotFound();

            } else {

                return user;
            }           
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("AddRoles")]
        public async Task<IActionResult> AddRoles(
            [FromBody] UpdateRoleRO addRolesRo) {
            
            await _userService.AddUserRoles(
                addRolesRo.UserId, 
                addRolesRo.RoleIds.ToList());

            return NoContent();
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            [FromBody] UpdateRoleRO addRolesRo) {
            
            await _userService.RemoveUserRoles(
                addRolesRo.UserId, 
                addRolesRo.RoleIds.ToList());

            return NoContent();
        }
    }
}
