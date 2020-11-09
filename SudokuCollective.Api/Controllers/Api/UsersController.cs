using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAppsService appsService;

        public UsersController(
            IUsersService userServ,
            IAppsService appsServ)
        {
            usersService = userServ;
            appsService = appsServ;
        }

        // POST: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id,
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                baseRequest.AppId,
                baseRequest.License,
                baseRequest.RequestorId))
            {
                var result = await usersService.GetUser(id, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: User Found";

                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                baseRequest.AppId,
                baseRequest.License,
                baseRequest.RequestorId))
            {
                var result = await usersService.GetUsers(baseRequest.PageListModel, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Users Found";

                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                updateUserRequest.AppId,
                updateUserRequest.License,
                updateUserRequest.RequestorId))
            {
                var result = await usersService.UpdateUser(id, updateUserRequest);

                if (result.Success)
                {
                    result.Message = "Status Code 200: User Updated";

                    return Ok(result);
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

        // PUT: api/users/5/updatepassword
        [Authorize(Roles = "USER")]
        [HttpPut("{id}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            int id, [FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                updatePasswordRequest.AppId,
                updatePasswordRequest.License,
                updatePasswordRequest.RequestorId))
            {
                var result = await usersService.UpdatePassword(id, updatePasswordRequest);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Password Updated";

                    return Ok(result);
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

        // DELETE: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(
            int id, [FromBody] BaseRequest baseRequest)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                baseRequest.AppId,
                baseRequest.License,
                baseRequest.RequestorId))
            {
                var result = await usersService.DeleteUser(id);

                if (result.Success)
                {
                    result.Message = "Status Code 200: User Deleted";

                    return Ok(result);
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

        // POST: api/users/addroles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}/AddRoles")]
        public async Task<IActionResult> AddRoles(
            int id,
            [FromBody] UpdateUserRoleRequest updateUserRoleRequest)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                updateUserRoleRequest.AppId,
                updateUserRoleRequest.License,
                updateUserRoleRequest.RequestorId))
            {
                var result = await usersService.AddUserRoles(
                    id,
                    updateUserRoleRequest.RoleIds.ToList());

                if (result.Success)
                {
                    result.Message = "Status Code 200: Roles Added";

                    return Ok(result);
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

        // DELETE: api/users/addroles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            int id,
            [FromBody] UpdateUserRoleRequest updateUserRoleRequest)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                updateUserRoleRequest.AppId,
                updateUserRoleRequest.License,
                updateUserRoleRequest.RequestorId))
            {
                var result = await usersService.RemoveUserRoles(
                    id,
                    updateUserRoleRequest.RoleIds.ToList());

                if (result.Success)
                {
                    result.Message = "Status Code 200: Roles Removed";

                    return Ok(result);
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

        // PUT: api/users/5/activateuser
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/ActivateUser")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await usersService.ActivateUser(id);

            if (result.Success)
            {
                result.Message = "Status Code 200: User Activated";

                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        // PUT: api/users/5/deactivateuser
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await usersService.DeactivateUser(id);

            if (result.Success)
            {
                result.Message = "Status Code 200: User Deactivated";

                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
    }
}
