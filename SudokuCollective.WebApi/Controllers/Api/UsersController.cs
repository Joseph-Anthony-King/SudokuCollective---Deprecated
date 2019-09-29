using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUsersService _userService;
        private readonly IAppsService _appsService;

        public UsersController(IUsersService userService,
            IAppsService appsService) {

            _userService = userService;
            _appsService = appsService;
        }

        // GET: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id, 
            [FromBody] BaseRequestRO baseRequestRO, 
            [FromQuery] bool fullRecord = true) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _userService.GetUser(id, fullRecord);

                if (result.Result) {

                    return Ok(result.User);

                } else {

                    return NotFound();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _userService.GetUsers(fullRecord);

                if (result.Result) {

                    return Ok(result.Users);

                } else {

                    return BadRequest("Issue obtaining users");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // PUT: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRO updateUserRO) {
            
            if (_appsService.ValidLicense(updateUserRO.License)) {
            
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

            } else {

                return BadRequest("Invalid License");
            }
        }

        // PUT: api/Users/UpdatePassword/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            int id, [FromBody] UpdatePasswordRO updatePasswordRO) {
            
            if (_appsService.ValidLicense(updatePasswordRO.License)) {

                var result = await _userService.UpdatePassword(id, updatePasswordRO);

                if (result) {

                    return Ok();

                } else {

                    return BadRequest("Issue updating password");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(
            int id, [FromBody] BaseRequestRO baseRequestRO) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {
            
                var result = await _userService.DeleteUser(id);

                if (result) {

                    return Ok();

                } else {

                    return NotFound();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}/AddRoles")]
        public async Task<IActionResult> AddRoles(
            int id,
            [FromBody] UpdateUserRolesRO updateUserRoles) {
            
            if (_appsService.ValidLicense(updateUserRoles.License)) {
            
                var result = await _userService.AddUserRoles(
                    id,
                    updateUserRoles.RoleIds.ToList());

                if (result) {

                    return Ok();

                } else {

                    return NotFound();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            int id,
            [FromBody] UpdateUserRolesRO updateUserRoles) {
            
            if (_appsService.ValidLicense(updateUserRoles.License)) {
            
                var result = await _userService.RemoveUserRoles(
                    id, 
                    updateUserRoles.RoleIds.ToList());

                if (result) {

                    return Ok();

                } else {

                    return NotFound();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }
    }
}
