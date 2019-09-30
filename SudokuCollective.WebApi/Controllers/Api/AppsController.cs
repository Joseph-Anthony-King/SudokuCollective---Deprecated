using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {
    
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/[controller]")]
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
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {
                
                var result = await _appsService.GetApp(id, fullRecord);

                if (result.Result) {

                    return Ok(result.App);

                } else {

                    return BadRequest("Error getting app");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }
        
        // GET: api/Apps/GetByLicense
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet, Route("GetByLicense")]
        public async Task<ActionResult<App>> GetApp(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {
                
                var result = await _appsService
                    .GetAppByLicense(baseRequestRO.License, fullRecord);

                if (result.Result) {

                    return Ok(result.App);

                } else {

                    return BadRequest("Error creating app");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App>>> GetApps(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequestRO.License)) {
                
                var result = await _appsService.GetApps();

                if (result.Result) {

                    return Ok(result.Apps);

                } else {

                    return BadRequest("Error creating app");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // Put: api/Apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut]
        public async Task<IActionResult> UpdateApp(
            [FromBody] UpdateAppRO updateAppRO) {
            
            if (_appsService.ValidLicense(updateAppRO.License)) {
                
                var result = await _appsService.UpdateApp(updateAppRO);

                if (result) {

                    return Ok();

                } else {

                    return BadRequest("Error updating app");
                }

            } else {

                return BadRequest("Invalid License");
            }            
        }

        // GET: api/Apps/GetUsers
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromBody] BaseRequestRO baseRequest,
            [FromQuery] bool fullRecord = true) {
            
            if (_appsService.ValidLicense(baseRequest.License)) {
                
                var result = await _appsService
                    .GetAppUsers(baseRequest, fullRecord);

                if (result.Result) {

                    return Ok(result.Users);

                } else {

                    return BadRequest("Error getting app users");
                }

            } else {

                return BadRequest("Invalid License");
            }  
        }
    }
}
