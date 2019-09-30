using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionsController : ControllerBase {

        private readonly ISolutionsService _solutionService;
        private readonly IAppsService _appsService;

        public SolutionsController(ISolutionsService solutionService, 
            IAppsService appsService) {
            
            _solutionService = solutionService;
            _appsService = appsService;
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuSolution>> GetSolution(
            int id,
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _solutionService.GetSolution(id, fullRecord);

                if (result.Result) {

                    return Ok(result.Solution);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>>  GetSolutions(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = true) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _solutionService.GetSolutions(fullRecord);

                if (result.Result) {

                    return Ok(result.Solutions);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolveRequestsRO solveRequestsRO) {
                
            if (_appsService.ValidLicense(solveRequestsRO.License)) {

                var result = await _solutionService.Solve(solveRequestsRO);

                if (result.Result) {

                    return Ok(result.Solution);

                } else {

                    return Ok("Requires more values in order to solve this puzzle");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }
    }
}
