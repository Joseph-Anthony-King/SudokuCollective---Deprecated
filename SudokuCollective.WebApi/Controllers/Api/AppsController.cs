using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase {

        private readonly IAppsService _appsService;

        public AppsController(IAppsService appsService) {

            _appsService = appsService;
        }
        
        // GET: api/Apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet, Route("{id}")]
        public async Task<ActionResult<App>> GetApp(
            int id,
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {
                
                var result = await _appsService.GetApp(id, fullRecord);

                if (result.Success) {

                    return Ok(result.App);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
        
        // GET: api/Apps/GetByLicense
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet, Route("GetByLicense")]
        public async Task<ActionResult<App>> GetAppByLicense(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {
                
                var result = await _appsService
                    .GetAppByLicense(baseRequest.License, fullRecord);

                if (result.Success) {

                    return Ok(result.App);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App>>> GetApps(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {
                
                var result = await _appsService
                    .GetApps(baseRequest.PageListModel, fullRecord);

                if (result.Success) {

                    return Ok(result.Apps);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // Put: api/Apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut]
        public async Task<IActionResult> UpdateApp(
            [FromBody] AppRequest appRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                appRequest.License,
                appRequest.RequestorId,
                appRequest.AppId)) {
                
                var result = await _appsService.UpdateApp(appRequest);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }            
        }

        // GET: api/Apps/GetUsers
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {
                
                var result = await _appsService
                    .GetAppUsers(baseRequest, fullRecord);

                if (result.Success) {

                    return Ok(result.Users);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }  
        }

        // PUT: api/Apps/AddUser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("AddUser/{userId}")]
        public async Task<IActionResult> AddUser(int userId, BaseRequest baseRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _appsService.AddAppUser(userId, baseRequest);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Apps/RemoveUser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(int userId, BaseRequest baseRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _appsService.RemoveAppUser(userId, baseRequest);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // Put: api/Apps/5/ActivateApp
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/ActivateApp")]
        public async Task<IActionResult> ActivateApp(int id) {            
                
            var result = await _appsService.ActivateApp(id);

            if (result.Success) {

                return Ok();

            } else {

                return NotFound(result.Message);
            }         
        }

        // Put: api/Apps/5/DeactivateApp
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut, Route("{id}/DeactivateApp")]
        public async Task<IActionResult> DeactivateApp(int id) {            
                
            var result = await _appsService.DeactivateApp(id);

            if (result.Success) {

                return Ok();

            } else {

                return NotFound(result.Message);
            }
        }

        // GET: api/Apps/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete, Route("{id}/DeleteApp")]
        public async Task<ActionResult> DeleteApp(int id) {

            var result = await _appsService.DeleteApp(id);

            if (result.Success) {

                return Ok(result.Message);

            } else {

                return NotFound(result.Message);
            }
        }

        // GET: api/Apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/ResetApp")]
        public async Task<ActionResult> ResetApp(
            int id,
            [FromBody] BaseRequest baseRequest) {

            if (await _appsService.IsOwnerOfThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _appsService.DeleteApp(id, true);

                if (result.Success) {

                    return Ok(result.Message);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("You are not the owner of this app");
            }
        }
    }
}
