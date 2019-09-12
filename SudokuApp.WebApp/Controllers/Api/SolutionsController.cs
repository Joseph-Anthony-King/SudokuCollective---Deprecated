using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.SolveRequests;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

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

            var solution = await _solutionService.GetSolution(id, fullRecord);

            if (solution == null)
            {
                return NotFound();
            }

            return solution;
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>>  GetSolutions(
            [FromQuery] bool fullRecord = true) {

            var solutions = await _solutionService.GetSolutions(fullRecord);

            if (solutions == null)
            {
                return NotFound();
            }

            return solutions;
        }

        // GET: api/solutions/solve
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("Solve")]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolveRequestsRO solveRequestsRO) {

            var solution = await _solutionService.Solve(solveRequestsRO);

            if (solution == null)
            {
                return NotFound();
            }

            return solution;
        }
    }
}