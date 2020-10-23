using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Api.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
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
        [HttpPost("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id, 
            [FromBody] BaseRequest baseRequest, 
            [FromQuery] bool fullRecord = false) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

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
        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _userService.GetUsers(baseRequest.PageListModel, fullRecord);

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
            int id, [FromBody] UpdateUserRequest updateUserRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRequest.License,
                updateUserRequest.RequestorId,
                updateUserRequest.AppId)) {
            
                var result = await _userService.UpdateUser(id, updateUserRequest);

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
            int id, [FromBody] UpdatePasswordRequest updatePasswordRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updatePasswordRequest.License,
                updatePasswordRequest.RequestorId,
                updatePasswordRequest.AppId)) {

                var result = await _userService.UpdatePassword(id, updatePasswordRequest);

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
            int id, [FromBody] BaseRequest baseRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {
            
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
            [FromBody] UpdateUserRoleRequest updateUserRoleRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRoleRequest.License,
                updateUserRoleRequest.RequestorId,
                updateUserRoleRequest.AppId)) {
            
                var result = await _userService.AddUserRoles(
                    id,
                    updateUserRoleRequest.RoleIds.ToList());

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
            [FromBody] UpdateUserRoleRequest updateUserRoleRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateUserRoleRequest.License,
                updateUserRoleRequest.RequestorId,
                updateUserRoleRequest.AppId)) {
            
                var result = await _userService.RemoveUserRoles(
                    id,
                    updateUserRoleRequest.RoleIds.ToList());

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
