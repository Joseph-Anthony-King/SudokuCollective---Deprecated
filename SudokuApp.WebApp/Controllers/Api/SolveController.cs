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
    public class SolveController : ControllerBase {

        private readonly ISolveMatrixService _solveMatrixService;

        public SolveController(ISolveMatrixService solveMatrixService) {
            
            _solveMatrixService = solveMatrixService;
        }

        // GET: api/solve
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<Game>> Solve(SolveRequestsRO solveRequestsRO) {

            var game = await _solveMatrixService.Solve(solveRequestsRO);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }
    }
}