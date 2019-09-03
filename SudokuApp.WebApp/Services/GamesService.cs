using System.Collections.Generic;
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

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public async Task<Game> DeleteGame(int gameId) {

            var game = await _context.Games.FindAsync(gameId);

            if (game == null) {

                return game = new Game();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public Task<ActionResult<Game>> GetGame(int gameId) {

            throw new System.NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<Game>>> GetGames() {

            throw new System.NotImplementedException();
        }

        public Task<ActionResult<Game>> GetMyGame(int userId, int gameId) {

            throw new System.NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId) {

            throw new System.NotImplementedException();
        }

        public Task<Game> DeleteMyGame(int userId, int gameId) {

            throw new System.NotImplementedException();
        }

        public async Task UpdateGame(int gameId, Game game) {

            if (gameId == game.Id) {

                _context.Entry(game).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }
    }
}