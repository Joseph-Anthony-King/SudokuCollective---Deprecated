using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionsController : ControllerBase {

        private readonly ISolutionsService _solutionService;

        public SolutionsController(ISolutionsService solutionService) {
            
            _solutionService = solutionService;
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuSolution>> GetSolution(
            int id, [FromQuery] bool fullRecord = true) {

            var result = await _solutionService.GetSolution(id, fullRecord);

            if (result.Result) {

                return Ok(result.Solution);

            } else {

                return BadRequest();
            }
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>>  GetSolutions(
            [FromQuery] bool fullRecord = true) {

            var result = await _solutionService.GetSolutions(fullRecord);

            if (result.Result) {

                return Ok(result.Solutions);

            } else {

                return BadRequest();
            }
        }

        // GET: api/solutions/solve
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("Solve")]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolveRequestsRO solveRequestsRO) {

            var result = await _solutionService.Solve(solveRequestsRO);

            if (result.Result) {

                return Ok(result.Solution);

            } else {

                return BadRequest();
            }
        }
    }
}
