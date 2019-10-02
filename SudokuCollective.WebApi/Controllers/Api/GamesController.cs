using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase {

        private readonly IGamesService _gamesService;
        private readonly IAppsService _appsService;

        public GamesController(IGamesService gamesService, 
            IAppsService appsService) {
            
            _gamesService = gamesService;
            _appsService = appsService;
        }

        // GET: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id, 
            [FromBody] BaseRequestRO baseRequestRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _gamesService.GetGame(id);

                if (result.Success) {
                    
                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromBody] BaseRequestRO baseRequestRO, 
            [FromQuery] bool fullRecord = true) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _gamesService.GetGames(fullRecord);

                if (result.Success) {

                    return Ok(result.Games);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(
            int id,
            [FromBody] BaseRequestRO baseRequestRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _gamesService.DeleteGame(id);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            int id, 
            [FromBody] UpdateGameRO updateGameRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                updateGameRO.License,
                updateGameRO.RequestorId)) {

                if (id != updateGameRO.GameId) {

                    return BadRequest("Id is incorrect");
                }

                var result = 
                    await _gamesService.UpdateGame(id, updateGameRO);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            } 
        }

        // POST: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(
            [FromBody] CreateGameRO createGameRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                createGameRO.License,
                createGameRO.RequestorId)) {
            
                var result = await _gamesService.CreateGame(createGameRO);

                if (result.Success) {

                    return CreatedAtAction(
                        "GetUser",
                        "Users",
                        new { id = result.Game.Id },
                        result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Games/5/CheckGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/CheckGame")]
        public async Task<ActionResult<Game>> CheckGame(
            int id,
            [FromBody] UpdateGameRO updateGameRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                updateGameRO.License, 
                updateGameRO.RequestorId)) {

                var result = await _gamesService.CheckGame(id, updateGameRO);

                if (result.Success) {
                    
                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
        
        // GET: api/Games/5/GetMyGame
        [Authorize(Roles = "USER")]
        [HttpGet, Route("{id}/GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            int id,
            [FromBody] GetMyGameRO getMyGameRO, 
            [FromQuery] bool fullRecord = true) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRO.License, 
                getMyGameRO.RequestorId)) {

                var result = await _gamesService.GetMyGame(getMyGameRO.UserId, id, fullRecord);

                if (result.Success) {

                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Games/GetMyGames
        [Authorize(Roles = "USER")]
        [HttpGet, Route("GetMyGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromBody] GetMyGameRO getMyGameRO,
            [FromQuery] bool fullRecord = true) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRO.License, 
                getMyGameRO.RequestorId)) {

                var result = await _gamesService.GetMyGames(getMyGameRO.UserId, fullRecord);

                if (result.Success) {

                    return Ok(result.Games);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Games/5/DeleteMyGame
        [Authorize(Roles = "USER")]
        [HttpDelete("{id}/DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            int id,
            [FromBody] GetMyGameRO getMyGameRO) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRO.License, 
                getMyGameRO.RequestorId)) {
                
                var result = await _gamesService.DeleteMyGame(
                    getMyGameRO.UserId, 
                    id);

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
