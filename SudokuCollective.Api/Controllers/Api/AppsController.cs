using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly IAppsService appsService;

        public AppsController(IAppsService appsServ)
        {
            appsService = appsServ;
        }

        // POST: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}")]
        public async Task<ActionResult<App>> GetApp(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.GetApp(id, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: App Found";

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

        // POST: api/apps/GetByLicense
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("GetByLicense")]
        public async Task<ActionResult<App>> GetAppByLicense(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetAppByLicense(request.License, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: App Found";

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

        // POST: api/apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<App>>> GetApps(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetApps(request.PageListModel, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Apps Found";

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

        // PUT: api/apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut]
        public async Task<IActionResult> UpdateApp(
            [FromBody] AppRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.UpdateApp(request);

                if (result.Success)
                {
                    result.Message = "Status Code 200: App Updated";

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

        // POST: api/apps/getusers
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost, Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetAppUsers(
                        request.AppId,
                        request.PageListModel, 
                        fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Users Found";

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

        // PUT: api/apps/adduser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("AddUser/{userId}")]
        public async Task<IActionResult> AddUser(int userId, [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.AddAppUser(userId, request);

                if (result.Success)
                {
                    result.Message = "Status Code 200: User Added";

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

        // DELETE: api/apps/removeuser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(int userId, [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.RemoveAppUser(userId, request);

                if (result.Success)
                {
                    result.Message = "Status Code 200: User Removed";

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

        // PUT: api/apps/5/activateapp
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/ActivateApp")]
        public async Task<IActionResult> ActivateApp(int id)
        {
            var result = await appsService.ActivateApp(id);

            if (result.Success)
            {
                result.Message = "Status Code 200: App Activated";

                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        // PUT: api/apps/5/deactivateapp
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/DeactivateApp")]
        public async Task<IActionResult> DeactivateApp(int id)
        {
            var result = await appsService.DeactivateApp(id);

            if (result.Success)
            {
                result.Message = "Status Code 200: App Deactivated";

                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        // DELETE: api/apps/5/deleteapp
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete, Route("{id}/DeleteApp")]
        public async Task<ActionResult> DeleteApp(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsOwnerOfThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.DeleteOrResetApp(id);

                if (result.Success)
                {
                    result.Message = "Status Code 200: App Deleted";

                    return Ok(result);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("You are not the owner of this app");
            }
        }

        // DELETE: api/apps/5/resetapp
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/ResetApp")]
        public async Task<ActionResult> ResetApp(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsOwnerOfThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.DeleteOrResetApp(id, true);

                if (result.Success)
                {
                    result.Message = "Status Code 200: App Reset";

                    return Ok(result);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("You are not the owner of this app");
            }
        }
    }
}
