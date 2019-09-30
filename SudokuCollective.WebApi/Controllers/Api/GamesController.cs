using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
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

            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _gamesService.GetGame(id);

                if (result.Result) {
                    
                    return Ok(result.Game);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromBody] BaseRequestRO baseRequestRO, 
            [FromQuery] bool fullRecord = true) {

            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _gamesService.GetGames(fullRecord);

                if (result.Result) {

                    return Ok(result.Games);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Games/GetMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("{id}/GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            int id,
            [FromBody] GetMyGameRO getMyGameRO, 
            [FromQuery] bool fullRecord = true) {

            if (_appsService.ValidLicense(getMyGameRO.License)) {

                var result = await _gamesService.GetMyGame(getMyGameRO.UserId, id, fullRecord);

                if (result.Result) {

                    return Ok(result.Game);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // GET: api/Games/GetMyGames/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetMyGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromBody] GetMyGameRO getMyGameRO,
            [FromQuery] bool fullRecord = true) {

            if (_appsService.ValidLicense(getMyGameRO.License)) {

                var result = await _gamesService.GetMyGames(getMyGameRO.UserId, fullRecord);

                if (result.Result) {

                    return Ok(result.Games);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // DELETE: api/Games/DeleteMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}/DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            int id,
            [FromBody] GetMyGameRO getMyGameRO) {

            if (_appsService.ValidLicense(getMyGameRO.License)) {
                
                var result = await _gamesService.DeleteMyGame(
                    getMyGameRO.UserId, 
                    id);

                if (result) {
                    
                    return Ok();

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // PUT: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            int id, 
            [FromBody] UpdateGameRO updateGameRO) {

            if (_appsService.ValidLicense(updateGameRO.License)) {

                if (id != updateGameRO.GameId) {

                    return BadRequest();
                }

                var result = 
                    await _gamesService.UpdateGame(id, updateGameRO);
                
                if (result.Result) {

                    return Ok();

                } else {

                    return BadRequest("Issue updating the game");
                }

            } else {

                return BadRequest("Invalid License");
            } 
        }

        // POST: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(
            [FromBody] CreateGameRO createGameRO) {

            if (_appsService.ValidLicense(createGameRO.License)) {
            
                var result = await _gamesService.CreateGame(createGameRO);

                if (result.Result) {

                    return CreatedAtAction(
                        "GetUser",
                        "Users",
                        new { id = result.Game.Id },
                        result.Game);

                } else {

                    return BadRequest("Error creating user");
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        // DELETE: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(
            int id,
            [FromBody] BaseRequestRO baseRequestRO) {

            if (_appsService.ValidLicense(baseRequestRO.License)) {

                var result = await _gamesService.DeleteGame(id);

                if (result) {

                    return Ok();

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }

        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/CheckGame")]
        public async Task<ActionResult<Game>> CheckGame(
            int id,
            [FromBody] UpdateGameRO updateGameRO) {

            if (_appsService.ValidLicense(updateGameRO.License)) {

                var result = await _gamesService.CheckGame(id, updateGameRO);

                if (result.Result) {
                    
                    return Ok(result.Game);

                } else {

                    return BadRequest();
                }

            } else {

                return BadRequest("Invalid License");
            }
        }
    }
}
