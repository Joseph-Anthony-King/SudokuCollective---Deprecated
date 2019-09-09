using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Helpers;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Models.RequestObjects.GameRequests;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class GamesService : IGamesService {

        private readonly ApplicationDbContext _context;
        private readonly IUsersService _userService;
        private readonly IDifficultiesService _difficultiesService;

        public GamesService(
            ApplicationDbContext context,
            IUsersService usersService,
            IDifficultiesService difficultiesService) {

            _context = context;
            _userService = usersService;
            _difficultiesService = difficultiesService;
        }

        public async Task<Game> CreateGame(CreateGameRO createGameRO) {

            var userActionResult = 
                await _userService.GetUser(createGameRO.UserId);
            var difficultyActionResult = 
                await _difficultiesService.GetDifficulty(createGameRO.DifficultyId);

            SudokuMatrix matrix = new SudokuMatrix();
            matrix.GenerateSolution();

            Game game = new Game(
                userActionResult.Value, 
                matrix, 
                difficultyActionResult.Value);

            _context.Games.Update(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public async Task UpdateGame(int id, Game game) {

            if (id == game.Id) {

                _context.Entry(game).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Game> DeleteGame(int id) {

            var game = await _context.Games.FindAsync(id);

            if (game == null) {

                return game = new Game();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public async Task<ActionResult<Game>> GetGame(int id) {

            var game = await _context.Games
                .Include(g => g.User).ThenInclude(u => u.Roles)
                .Include(g => g.SudokuMatrix)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) {

                return game = new Game();
            }

            game.SudokuMatrix.SudokuCells = await StaticApiHelpers.ResetSudokuCells(game, _context);

            return game;
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetGames() {

            var games = await _context.Games
                .OrderBy(g => g.Id)
                .Include(g => g.User).ThenInclude(u => u.Roles)
                .Include(g => g.SudokuMatrix)
                .ToListAsync();
            
            foreach (var game in games) {
                
                game.SudokuMatrix.SudokuCells = await StaticApiHelpers.ResetSudokuCells(game, _context);
            }

            return games;
        }

        public async Task<ActionResult<Game>> GetMyGame(int userId, int gameId) {

            var game = await _context.Games
                .Include(g => g.User).ThenInclude(u => u.Roles)
                .Include(g => g.SudokuMatrix)
                .FirstOrDefaultAsync(g => g.User.Id == userId && g.Id == gameId);

            if (game == null) {

                return game = new Game();
            }

            game.SudokuMatrix.SudokuCells = await StaticApiHelpers.ResetSudokuCells(game, _context);

            return game;
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId) {

            var games = await _context.Games
                .Where(g => g.User.Id == userId)
                .OrderBy(g => g.Id)
                .Include(g => g.User).ThenInclude(u => u.Roles)
                .Include(g => g.SudokuMatrix)
                .ToListAsync();
            
            foreach (var game in games) {

                game.SudokuMatrix.SudokuCells = await StaticApiHelpers.ResetSudokuCells(game, _context);
            }

            return games;
        }

        public async Task<Game> DeleteMyGame(int userId, int gameId) {

            var game = await _context.Games
                .FirstOrDefaultAsync(predicate: g => g.Id == gameId && g.User.Id == userId);

            if (game == null) {

                return game = new Game();
            }

            return game;
        }
    }
}
