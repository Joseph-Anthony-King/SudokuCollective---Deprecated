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
                    result.Message = "Status Code 200: Solution Found";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 404: Solution Not Found";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
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
                    result.Message = "Status Code 200: Solutions Found";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 404: Solutions Not Found";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // POST: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost, Route("Solve")]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolveRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await solutionService.Solve(request);

                if (result.Success)
                {
                    if (result.Solution != null)
                    {
                        result.Message = "Status Code 200: Sudoku Solved";

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Status Code 404: Sudoku Not Solved";

                        return Ok(result);
                    }
                }
                else
                {
                    result.Message = "Status Code 404: Sudoku Not Solved";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }

        // POST: api/solutions/generate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("Generate")]
        public async Task<ActionResult<SudokuSolution>> Generate(
            [FromBody] SolveRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await solutionService.Generate();

                if (result.Success)
                {
                    result.Message = "Status Code 200: Sudoku Generated";

                    return Ok(result);
                }
                else
                {
                    result.Message = "Status Code 200: Sudoku Not Generated";

                    return NotFound(result);
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
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
                        result.Message = "Status Code 200: Sudoku Solutions Added";

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = "Status Code 200: Sudoku Solutions Not Added";

                        return NotFound(result);
                    }
                }
                else
                {
                    return BadRequest(
                        string.Format(
                            "Status Code 400: The amount of solutions requested, {0}, exceeds the service's 1,000 solution limit",
                            request.Limit.ToString())
                        );
                }
            }
            else
            {
                return BadRequest("Status Code 400: Invalid Request on this License");
            }
        }
    }
}
