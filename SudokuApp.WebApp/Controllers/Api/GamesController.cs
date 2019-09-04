using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase {

        private readonly IGamesService _gamesService;
        private readonly IUsersService _userService;
        private readonly IDifficultiesService _difficultiesService;

        public GamesController(
            IGamesService gamesService,
            IUsersService usersService,
            IDifficultiesService difficultiesService) {
            
            _gamesService = gamesService;
            _userService = usersService;
            _difficultiesService = difficultiesService;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames() {

            return await _gamesService.GetGames();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id) {

            var game = await _gamesService.GetGame(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            int id, 
            Game game) {

            if (id != game.Id) {

                return BadRequest();
            }

            await _gamesService.UpdateGame(id, game);

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(
            int userId,
            int difficultyId) {

            var userActionResult = await _userService.GetUser(userId);
            var difficultyActionResult = await _difficultiesService.GetDifficulty(difficultyId);
            
            var game = await _gamesService.CreateGame(
                userActionResult.Value, 
                difficultyActionResult.Value);

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id) {

           var game = await _gamesService.DeleteGame(id);

           return game;
        }
    }
}
