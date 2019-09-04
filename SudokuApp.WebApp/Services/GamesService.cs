using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class GamesService : IGamesService {

        private readonly ApplicationDbContext _context;

        public GamesService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<Game> CreateGame(User user, Difficulty difficulty) {

            SudokuMatrix matrix = new SudokuMatrix();
            matrix.GenerateSolution();

            Game game = new Game(user, matrix, difficulty);

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
                .Include(g => g.User).ThenInclude(u => u.Permissions)
                .Include(g => g.SudokuMatrix)
                .FirstOrDefaultAsync(g => g.Id == id);

            // Reset the matrix sudoku cells
            game.SudokuMatrix.SudokuCells = null;
            var cells = await _context.SudokuCells.Where(cell => cell.SudokuMatrix.Id == game.SudokuMatrixId).OrderBy(cell => cell.Index).ToListAsync();
            game.SudokuMatrix.SudokuCells = cells;

            if (game == null) {

                return game = new Game();
            }

            return game;
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetGames() {

            return await _context.Games.ToListAsync();
        }

        public async Task<ActionResult<Game>> GetMyGame(int userId) {

            var game = await _context.Games.FirstOrDefaultAsync(g => g.User.Id == userId);

            if (game == null) {

                return game = new Game();
            }

            return game;
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId) {

            return await _context.Games.Where(g => g.User.Id == userId).ToListAsync();
        }

        public async Task<Game> DeleteMyGame(int userId, int gameId) {

            var game = await _context.Games.FirstOrDefaultAsync(predicate: g => g.Id == gameId && g.User.Id == userId);

            if (game == null) {

                return game = new Game();
            }

            return game;
        }
    }
}
