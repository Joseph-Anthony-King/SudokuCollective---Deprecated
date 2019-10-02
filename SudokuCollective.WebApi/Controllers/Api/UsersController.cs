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

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
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
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _userService.GetUser(id, fullRecord);

                if (result.Success) {

                    return Ok(result.User);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Users
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _userService.GetUsers(fullRecord);

                if (result.Success) {

                    return Ok(result.Users);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRO updateUserRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRO.License, 
                updateUserRO.RequestorId)) {
            
                var result = await _userService.UpdateUser(id, updateUserRO);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Users/5/UpdatePassword
        [Authorize(Roles = "USER")]
        [HttpPut("{id}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            int id, [FromBody] UpdatePasswordRO updatePasswordRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updatePasswordRO.License, 
                updatePasswordRO.RequestorId)) {

                var result = await _userService.UpdatePassword(id, updatePasswordRO);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(
            int id, [FromBody] BaseRequestRO baseRequestRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {
            
                var result = await _userService.DeleteUser(id);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}/AddRoles")]
        public async Task<IActionResult> AddRoles(
            int id,
            [FromBody] UpdateUserRolesRO updateUserRolesRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRolesRO.License, 
                updateUserRolesRO.RequestorId)) {
            
                var result = await _userService.AddUserRoles(
                    id,
                    updateUserRolesRO.RoleIds.ToList());

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // api/Users/AddRoles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            int id,
            [FromBody] UpdateUserRolesRO updateUserRolesRO) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRolesRO.License, 
                updateUserRolesRO.RequestorId)) {
            
                var result = await _userService.RemoveUserRoles(
                    id, 
                    updateUserRolesRO.RoleIds.ToList());

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // Put: api/Users/5/ActivateUser
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/ActivateUser")]
        public async Task<IActionResult> ActivateUser(int id) {            
                
            var result = await _userService.ActivateUser(id);

            if (result.Success) {

                return Ok();

            } else {

                return NotFound(result.Message);
            }         
        }

        // Put: api/Users/5/DeactivateUser
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(int id) {            
                
            var result = await _userService.DeactivateUser(id);

            if (result.Success) {

                return Ok();

            } else {

                return NotFound(result.Message);
            }         
        }
    }
}
