using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
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

            var result = await _userService.GetUser(id, fullRecord);

            if (result.Result) {

                return Ok(result.User);

            } else {

                return NotFound();
            }
        }

        // GET: api/Users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromQuery] bool fullRecord = true) {

            var result = await _userService.GetUsers(fullRecord);

            if (result.Result) {

                return Ok(result.Users);

            } else {

                return BadRequest("Issue obtaining users");
            }
        }

        // PUT: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRO updateUserRO) {
            
            var result = await _userService.UpdateUser(id, updateUserRO);

            if (result.Result) {

                return Ok();

            } else {

                if (result.User.Email.Equals(updateUserRO.Email)
                    && result.User.Id == 0) {

                    return BadRequest("Email is not unique");

                } else {

                    return BadRequest();
                }
            }
        }

        // PUT: api/Users/UpdatePassword/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(
            int id, [FromBody] UpdatePasswordRO updatePasswordRO) {

            var result = await _userService.UpdatePassword(id, updatePasswordRO);

            if (result) {

                return Ok();

            } else {

                return BadRequest("Issue updating password");
            }
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            
            var result = await _userService.DeleteUser(id);

            if (result) {

                return Ok();

            } else {

                return NotFound();
            }           
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("AddRoles")]
        public async Task<IActionResult> AddRoles(
            [FromBody] UpdateRoleRO addRolesRo) {
            
            var result = await _userService.AddUserRoles( 
                addRolesRo.UserId,
                addRolesRo.RoleIds.ToList());

            if (result) {

                return Ok();

            } else {

                return NotFound();
            }
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            [FromBody] UpdateRoleRO addRolesRo) {
            
            var result = await _userService.RemoveUserRoles(
                addRolesRo.UserId, 
                addRolesRo.RoleIds.ToList());

            if (result) {

                return Ok();

            } else {

                return NotFound();
            }
        }
    }
}
