using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DTO;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers
{

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

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames() {

            return await _gamesService.GetGames();
        }

        // GET: api/Games/GetMyGame
        [HttpGet, Route("GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame([FromBody] GetMyGameDTO getMyGameDTO) {

            var game = await _gamesService.GetMyGame(getMyGameDTO.UserId, getMyGameDTO.GameId);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET: api/Games/GetMyGames/5
        [HttpGet, Route("GetMyGames/{userId}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId) {

            return await _gamesService.GetMyGames(userId);
        }

        // DELETE: api/Games/DeleteMyGame
        [HttpDelete("DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame([FromBody] GetMyGameDTO getMyGameDTO) {

           var game = await _gamesService.DeleteMyGame(getMyGameDTO.UserId, getMyGameDTO.GameId);

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
        public async Task<ActionResult<Game>> PostGame([FromBody] CreateGameDTO createGame) {

            var userActionResult = await _userService.GetUser(createGame.UserId);
            var difficultyActionResult = await _difficultiesService.GetDifficulty(createGame.DifficultyId);
            
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
