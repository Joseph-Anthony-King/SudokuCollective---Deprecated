using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
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
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _difficultiesService.GetDifficulty(id, fullRecord);

                if (result.Success) {

                    return Ok(result.Difficulty);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Difficulties
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            [FromBody] BaseRequestRO baseRequestRO, [FromQuery] bool fullRecord = true) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _difficultiesService.GetDifficulties(fullRecord);

                if (result.Success) {

                    return Ok(result.Difficulties);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id, 
            [FromBody] UpdateDifficultyRO updateDifficultyRO) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                updateDifficultyRO.License, 
                updateDifficultyRO.RequestorId)) {

                if (id != updateDifficultyRO.Id) {

                    return BadRequest("Invalid Request: Difficulty Id Incorrect");
                }
                
                var result = await _difficultiesService.UpdateDifficulty(id, updateDifficultyRO);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }         
        }

        // POST: api/Difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Difficulty>> PostDifficulty(
            [FromBody] CreateDifficultyRO createDifficultyRO) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                createDifficultyRO.License, 
                createDifficultyRO.RequestorId)) {  
            
                var result = await _difficultiesService
                    .CreateDifficulty(createDifficultyRO.Name, createDifficultyRO.DifficultyLevel);

                if (result.Success) {

                    return CreatedAtAction(
                        "GetDifficulty", 
                        new { id = result.Difficulty.Id }, 
                        result.Difficulty);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDifficulty(int id, 
            [FromBody] BaseRequestRO baseRequestRO) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _difficultiesService.DeleteDifficulty(id);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }
                
            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
