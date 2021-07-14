using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SolutionsController : ControllerBase
    {
        private readonly ISolutionsService solutionService;
        private readonly IAppsService appsService;

        public SolutionsController(ISolutionsService solutionServ,
            IAppsService appsServ)
        {
            solutionService = solutionServ;
            appsService = appsServ;
        }

        // POST: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<SudokuSolution>> GetSolution(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await solutionService.GetSolution(id, fullRecord);

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

        // POST: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>> GetSolutions(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await solutionService
                    .GetSolutions(request, fullRecord);

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

        // POST: api/solutions
        [AllowAnonymous]
        [HttpPost, Route("Solve")]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolutionRequest request)
        {
            var result = await solutionService.Solve(request);

            if (result.Success)
            {
                if (result.Solution != null)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
            }
        }

        // POST: api/solutions/generate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("Generate")]
        public async Task<ActionResult<SudokuSolution>> Generate()
        {
            var result = await solutionService.Generate();

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

        // POST: api/solutions/addsolutions
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost, Route("AddSolutions")]
        public async Task<IActionResult> AddSolutions(
            [FromBody] AddSolutionRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                if (request.Limit <= 1000)
                {
                    var result = await solutionService.AddSolutions(request.Limit);

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
                    return BadRequest(
                        ControllerMessages.StatusCode400(
                            string.Format(
                                "The Amount Of Solutions Requested, {0}, Exceeds The Service's 1,000 Limit",
                                request.Limit.ToString())
                            ));
                }
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }
    }
}
