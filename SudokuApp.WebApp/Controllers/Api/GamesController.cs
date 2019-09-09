using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.GameRequests;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

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
        public async Task<ActionResult<Game>> GetGame(int id) {

            var game = await _gamesService.GetGame(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames() {

            return await _gamesService.GetGames();
        }

        // GET: api/Games/GetMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame([FromBody] GetMyGameRO getMyGameRO) {

            var game = await _gamesService.GetMyGame(getMyGameRO.UserId, getMyGameRO.GameId);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET: api/Games/GetMyGames/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("GetMyGames/{userId}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId) {

            return await _gamesService.GetMyGames(userId);
        }

        // DELETE: api/Games/DeleteMyGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame([FromBody] GetMyGameRO getMyGameRO) {

           var game = await _gamesService.DeleteMyGame(getMyGameRO.UserId, getMyGameRO.GameId);

           return game;
        }

        // PUT: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            int id, 
            [FromBody] Game game) {

            if (id != game.Id) {

                return BadRequest();
            }

            await _gamesService.UpdateGame(id, game);

            return NoContent();
        }

        // POST: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame([FromBody] CreateGameRO createGameRO) {
            
            var game = await _gamesService.CreateGame(createGameRO);

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id) {

           var game = await _gamesService.DeleteGame(id);

           return game;
        }
    }
}
