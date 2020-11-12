using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using System.Net;

namespace SudokuCollective.Api.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase
    {
        private readonly IDifficultiesService difficultiesService;
        private readonly IAppsService appsService;

        public DifficultiesController(
            IDifficultiesService difficultiesServ,
            IAppsService appsServ)
        {
            difficultiesService = difficultiesServ;
            appsService = appsServ;
        }

        // POST: api/difficulties/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Difficulty>> GetDifficulty(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await difficultiesService.GetDifficulty(id, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Difficulty Found";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 404: Difficulty Not Found";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // POST: api/difficulties
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            [FromBody] BaseRequest request, [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await difficultiesService
                    .GetDifficulties(request.PageListModel, fullRecord);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Difficulties Found";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 404: Difficulties Not Found";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // PUT: api/difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id,
            [FromBody] UpdateDifficultyRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                if (id != request.Id)
                {
                    return BadRequest("Invalid Request: Difficulty Id Incorrect");
                }

                var result = await difficultiesService.UpdateDifficulty(id, request);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Difficulty Updated";

                    return Ok();
                }
                else
                {
                    result.Message = "Status Code 404: Difficulty Not Updated";

                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // POST: api/difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost, Route("Create")]
        public async Task<ActionResult<Difficulty>> PostDifficulty(
            [FromBody] CreateDifficultyRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await difficultiesService
                    .CreateDifficulty(request.Name, request.DisplayName, request.DifficultyLevel);

                if (result.Success)
                {
                    result.Message = "Status Code 201: Difficulty Created";

                    return StatusCode((int)HttpStatusCode.Created, result);
                }
                else
                {
                    result.Message = "Status Code 404: Difficulty Not Created";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // DELETE: api/difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDifficulty(int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await difficultiesService.DeleteDifficulty(id);

                if (result.Success)
                {
                    result.Message = "Status Code 200: Difficulty Deleted";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 404: Difficulty Not Deleted";

                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }
    }
}
