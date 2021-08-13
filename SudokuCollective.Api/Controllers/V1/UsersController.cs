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
        public async Task<ActionResult<User>> Get(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.Get(
                    id, 
                    request.License);

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
        public async Task<IActionResult> Update(
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

                var result = await usersService.Update(
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

        // DELETE: api/users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(
            int id, [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.Delete(id, request.License);

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
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.GetUsers(
                    request.RequestorId, 
                    request.License,
                    request.Paginator);

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

        // PUT: api/users/addroles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/AddRoles")]
        public async Task<IActionResult> AddRoles(
            int id,
            [FromBody] UpdateUserRoleRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.AddUserRoles(
                    id,
                    request.RoleIds.ToList(),
                    request.License);

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

        // PUT: api/users/addroles
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/RemoveRoles")]
        public async Task<IActionResult> RemoveRoles(
            int id,
            [FromBody] UpdateUserRoleRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.RemoveUserRoles(
                    id,
                    request.RoleIds.ToList(),
                    request.License);

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

        // PUT: api/users/5/activate
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/Activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await usersService.Activate(id);

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

        // PUT: api/users/5/deactivate
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await usersService.Deactivate(id);

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

        // POST: api/users/requestPasswordReset
        [AllowAnonymous]
        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequest request)
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
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/password-reset-requested-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/confirm-old-email-inlined.html";
            }

            var result = await usersService.RequestPasswordReset(request, baseUrl, emailtTemplatePath);

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

        // PUT: api/users/resendRequestPasswordReset
        [AllowAnonymous]
        [HttpPut("ResendRequestPasswordReset")]
        public async Task<IActionResult> ResendRequestPasswordReset([FromBody] BaseRequest request)
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
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/password-reset-requested-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/confirm-old-email-inlined.html";
            }

            var result = await usersService.ResendPasswordReset(
                request.RequestorId,
                request.AppId,
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

        // PUT: api/users/cancelEmailConfirmation
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("cancelEmailConfirmation")]
        public async Task<IActionResult> CancelEmailConfirmation([FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.CancelEmailConfirmationRequest(request.RequestorId, request.AppId);

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

        // PUT: api/users/cancelPasswordReset
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("cancelPasswordReset")]
        public async Task<IActionResult> CancelPasswordReset([FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.CancelPasswordResetRequest(request.RequestorId, request.AppId);

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

        // PUT: api/users/cancelAllEmailRequests
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("cancelAllEmailRequests")]
        public async Task<IActionResult> CancelAllEmailRequests([FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await usersService.CancelAllEmailRequests(request.RequestorId, request.AppId);

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
    }
}
