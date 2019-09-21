using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase {

        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService) {
            
            _gamesService = gamesService;
        }

        // GET: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame([FromQuery] int id) {

            var game = await _gamesService.GetGame(id);

            if (_gamesService.IsGameValid(game)) {
                
                return game;

            } else {

                return BadRequest();
            }
        }

        // GET: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromQuery] bool fullRecord = true) {

            return await _gamesService.GetGames(fullRecord);
        }

        // GET: api/Games/GetMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            [FromBody] GetMyGameRO getMyGameRO, 
            [FromQuery] bool fullRecord = true) {

            var game = await _gamesService.GetMyGame(getMyGameRO.UserId, getMyGameRO.GameId, fullRecord);

            if (_gamesService.IsGameValid(game)) {
                
                return game;

            } else {

                return BadRequest();
            }
        }

        // GET: api/Games/GetMyGames/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetMyGames/{userId}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromQuery] int userId, 
            [FromQuery] bool fullRecord = true) {

            return await _gamesService.GetMyGames(userId, fullRecord);
        }

        // DELETE: api/Games/DeleteMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            [FromBody] GetMyGameRO getMyGameRO) {

           var game = await _gamesService.DeleteMyGame(getMyGameRO.UserId, getMyGameRO.GameId);

            if (_gamesService.IsGameValid(game)) {
                
                return game;

            } else {

                return BadRequest();
            }
        }

        // PUT: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            [FromQuery] int id, 
            [FromBody] UpdateGameRO updateGameRO) {

            if (id != updateGameRO.GameId) {

                return BadRequest();
            }

            var gameSuccessfullyUpdated = 
                await _gamesService.UpdateGame(id, updateGameRO);
            
            if (gameSuccessfullyUpdated) {

                return NoContent();

            } else {

                return BadRequest("Issue updating the game");
            }
        }

        // POST: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(
            [FromBody] CreateGameRO createGameRO) {
            
            var game = await _gamesService.CreateGame(createGameRO);

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame([FromQuery] int id) {

           var game = await _gamesService.DeleteGame(id);

            if (_gamesService.IsGameValid(game)) {
                
                return game;

            } else {

                return BadRequest();
            }
        }

        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("CheckGame")]
        public async Task<ActionResult<Game>> CheckGame([FromBody] UpdateGameRO checkGameRO) {

            var game = await _gamesService.CheckGame(checkGameRO);

            if (_gamesService.IsGameValid(game)) {
                
                return game;

            } else {

                return BadRequest();
            }            
        }
    }
}
