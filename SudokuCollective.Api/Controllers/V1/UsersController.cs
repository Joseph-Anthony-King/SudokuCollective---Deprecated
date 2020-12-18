using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using Microsoft.AspNetCore.Hosting;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAppsService appsService;
        private readonly IWebHostEnvironment hostEnvironment;

        public UsersController(
            IUsersService userServ,
            IAppsService appsServ,
            IWebHostEnvironment environment)
        {
            usersService = userServ;
            appsService = appsServ;
            hostEnvironment = environment;
        }

        // POST: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id,
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                baseRequest.AppId,
                baseRequest.License,
                baseRequest.RequestorId))
            {
                var result = await usersService.GetUser(id, fullRecord);

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST: api/users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                baseRequest.AppId,
                baseRequest.License,
                baseRequest.RequestorId))
            {
                var result = await usersService.GetUsers(baseRequest.PageListModel, fullRecord);

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] UpdateUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                string baseUrl;

                if (Request != null)
                {
                    baseUrl = Request.Host.ToString();
                }
                else
                {
                    baseUrl = "https://SudokuCollective.com";
                }

                string emailtTemplatePath;

                if (!string.IsNullOrEmpty(hostEnvironment.WebRootPath))
                {
                    emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/confirm-old-email-inlined.html");

                    emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
                }
                else
                {
                    emailtTemplatePath = "../../Content/EmailTemplates/confirm-old-email-inlined.html";
                }

                var result = await usersService.UpdateUser(
                    id, 
                    request,
                    baseUrl,
                    emailtTemplatePath);

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }

            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/users/requestPasswordUpdate
        [Authorize(Roles = "USER")]
        [HttpPut("RequestPasswordUpdate")]
        public async Task<IActionResult> RequestPasswordUpdate([FromBody] RequestPasswordUpdateRequest request)
        {
            string baseUrl;

            if (Request != null)
            {
                baseUrl = Request.Host.ToString();
            }
            else
            {
                baseUrl = "https://SudokuCollective.com";
            }

            string emailtTemplatePath;

            if (!string.IsNullOrEmpty(hostEnvironment.WebRootPath))
            {
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/update-password-requested-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/confirm-old-email-inlined.html";
            }

            var result = await usersService.RequestPasswordUpdate(request, baseUrl, emailtTemplatePath);

            if (result.Success)
            {
                result.Message = ControllerMessages.StatusCode200(result.Message);

                return Ok(result);
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
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
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
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
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
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
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
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
                result.Message = ControllerMessages.StatusCode200(result.Message);

                return Ok(result);
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
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
                result.Message = ControllerMessages.StatusCode200(result.Message);

                return Ok(result);
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
            }
        }
    }
}
