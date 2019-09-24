using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase {

        private readonly IDifficultiesService _difficultiesService;
        private readonly IAppsService _appsService;

        public DifficultiesController(IDifficultiesService difficultiesService,
            IAppsService appsService) {

            _difficultiesService = difficultiesService;
            _appsService = appsService;
        }

        // GET: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Difficulty>> GetDifficulty(
            int id, 
            [FromBody] BaseRequestRO baseRequestRO, 
            [FromQuery] bool fullRecord = true) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _difficultiesService.GetDifficulty(id, fullRecord);

                if (result.Result) {

                    return Ok(result.Difficulty);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Difficulties
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            [FromBody] BaseRequestRO baseRequestRO, [FromQuery] bool fullRecord = true) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _difficultiesService.GetDifficulties(fullRecord);

                if (result.Result) {

                    return Ok(result.Difficulties);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // PUT: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id, 
            [FromBody] UpdateDifficultyRO updateDifficultyRO) {
                
            if (_appsService.ValidLicense(updateDifficultyRO.License)) {

                if (id != updateDifficultyRO.Id) {

                    return BadRequest();
                }
                
                var result = await _difficultiesService.UpdateDifficulty(id, updateDifficultyRO);

                if (result) {

                    return Ok();

                } else {

                    return NoContent();
                }

            } else {

                return BadRequest("Invalid License");
            }         
        }

        // POST: api/Difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Difficulty>> PostDifficulty(
            [FromBody] CreateDifficultyRO createDifficultyRO) {
                
            if (_appsService.ValidLicense(createDifficultyRO.License)) {  
            
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

            } else {

                return BadRequest("Invalid License");
            }
        }

        // DELETE: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDifficulty(int id, 
            [FromBody] BaseRequestRO baseRequestRO) {
                
            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _difficultiesService.DeleteDifficulty(id);

                if (result) {

                    return Ok();

                } else {

                    return BadRequest();
                }
                
            } else {

                return BadRequest("Invalid License");
            }
        }
    }
}
