using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
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

            var result = await _difficultiesService.GetDifficulty(id, fullRecord);

            if (result.Result) {

                return Ok(result.Difficulty);

            } else {

                return BadRequest();
            }
        }

        // GET: api/Difficulties
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            [FromQuery] bool fullRecord = true) {

            var result = await _difficultiesService.GetDifficulties(fullRecord);

            if (result.Result) {

                return Ok(result.Difficulties);

            } else {

                return BadRequest();
            }
        }

        // PUT: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id, 
            [FromBody] Difficulty difficulty) {

            if (id != difficulty.Id) {

                return BadRequest();
            }
            
            var result = await _difficultiesService.UpdateDifficulty(id, difficulty);

            if (result) {

                return Ok();

            } else {

                return NoContent();
            }            
        }

        // POST: api/Difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Difficulty>> PostDifficulty(
            [FromBody] CreateDifficultyRO createDifficultyRO) {
            
            var result = await _difficultiesService
                .CreateDifficulty(createDifficultyRO.Name, createDifficultyRO.DifficultyLevel);

            if (result.Result) {

                return CreatedAtAction(
                    "GetDifficulty", 
                    new { id = result.Difficulty.Id }, 
                    result.Difficulty);

            } else {

                return BadRequest();
            }
        }

        // DELETE: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDifficulty(int id) {
            
            var result = await _difficultiesService.DeleteDifficulty(id);

            if (result) {

                return Ok();

            } else {

                return BadRequest();
            }
        }
    }
}
