using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase {

        private readonly IDifficultiesService _difficultiesService;

        public DifficultiesController(IDifficultiesService difficultiesService) {

            _difficultiesService = difficultiesService;
        }

        // GET: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Difficulty>> GetDifficulty(
            int id, [FromQuery] bool fullRecord = true) {

            var difficulty = await _difficultiesService.GetDifficulty(id, fullRecord);

            if (string.IsNullOrEmpty(difficulty.Value.Name)) {

                return BadRequest();

            } else {

                return difficulty;
            }
        }

        // GET: api/Difficulties
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            [FromQuery] bool fullRecord = true) {

            return await _difficultiesService.GetDifficulties(fullRecord);
        }

        // PUT: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id, 
            [FromBody] Difficulty difficulty) {

            if (id != difficulty.Id) {

                return BadRequest();
            }
            
            await _difficultiesService.UpdateDifficulty(id, difficulty);

            return NoContent();
        }

        // POST: api/Difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Difficulty>> PostDifficulty(
            [FromBody] CreateDifficultyRO createDifficultyRO) {
            
            var difficulty = await _difficultiesService
                .CreateDifficulty(createDifficultyRO.Name, createDifficultyRO.DifficultyLevel);

            return CreatedAtAction("GetDifficulty", new { id = difficulty.Id }, difficulty);
        }

        // DELETE: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Difficulty>> DeleteDifficulty(int id) {
            
            var difficulty = await _difficultiesService.DeleteDifficulty(id);

            return difficulty;
        }
    }
}
