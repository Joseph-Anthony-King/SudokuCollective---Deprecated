using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;

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
                    return BadRequest(ControllerMessages.IdIncorrectMessage);
                }

                var result = await difficultiesService.UpdateDifficulty(id, request);

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
                    result.Message = ControllerMessages.StatusCode201(result.Message);

                    return StatusCode((int)HttpStatusCode.Created, result);
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
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }
    }
}
