using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.TaskModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

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

        public async Task<GameTaskResult> CreateGame(CreateGameRO createGameRO) {

            var gameTaskResult = new GameTaskResult() {

                Result = false,
                Game = new Game() {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };

            try {

                var result = await _userService.GetUser(createGameRO.UserId);
                var difficultyActionResult =
                    await _difficultiesService.GetDifficulty(createGameRO.DifficultyId);

                SudokuMatrix matrix = new SudokuMatrix();
                matrix.GenerateSolution();

                var game = new Game(
                    result.User,
                    matrix,
                    difficultyActionResult.Value);

                var userRoles = await _context.UsersRoles
                    .Where(ur => ur.UserId == result.User.Id)
                    .ToListAsync();

                var userGames = await _context.Games
                    .Where(g => g.UserId == result.User.Id)
                    .ToListAsync();

                _context.Games.Update(game);
                await _context.SaveChangesAsync();

                _context.UsersRoles.AddRange(userRoles);
                await _context.SaveChangesAsync();

                _context.Games.UpdateRange(userGames);
                await _context.SaveChangesAsync();

                gameTaskResult.Result = true;
                gameTaskResult.Game = game;

                return gameTaskResult;

            } catch (Exception) {

                return gameTaskResult;
            }
        }

        public async Task<GameTaskResult> UpdateGame(int id, UpdateGameRO updateGameRO) {

            var gameTaskResult = new GameTaskResult() {

                Result = false,
                Game = new Game()
                {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };

            try {

                if (id == updateGameRO.GameId) {

                    var game = await _context.Games
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                            .FirstOrDefaultAsync(predicate: g => g.Id == updateGameRO.GameId);

                    game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                    game.IsSolved();

                    foreach (var cell in game.SudokuMatrix.SudokuCells) {

                        _context.SudokuCells.Update(cell);
                    }

                    await _context.SaveChangesAsync();

                    gameTaskResult.Result = true;
                    gameTaskResult.Game = game;

                }

                return gameTaskResult;

            } catch (Exception) {

                return gameTaskResult;
            }
        }

        public async Task<bool> DeleteGame(int id) {

            var result = false;

            try {

                var game = await _context.Games.FindAsync(id);

                if (game != null) {

                    _context.Games.Remove(game);
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<GameTaskResult> GetGame(int id) {

            var gameTaskResult = new GameTaskResult() {

                Result = false,
                Game = new Game() {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };

            try {

                var game = await _context.Games
                    .Include(g => g.User).ThenInclude(u => u.Roles)
                    .Include(g => g.SudokuMatrix)
                    .Include(g => g.SudokuSolution)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null) {

                    game = new Game()
                    {

                        Id = 0,
                        UserId = 0,
                        SudokuMatrixId = 0
                    };

                } else {

                    game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                    gameTaskResult.Result = true;
                    gameTaskResult.Game = game;
                }

                return gameTaskResult;

            } catch(Exception) {

                return gameTaskResult;
            }
        }

        public async Task<GameListTaskResult> GetGames(bool fullRecord = true) {

            var gameListTaskResult = new GameListTaskResult() {

                Result = false,
                Games = new List<Game>()
            };

            try {

                var games = new List<Game>();

                if (fullRecord) {

                    games = await _context.Games
                        .OrderBy(g => g.Id)
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .ToListAsync();

                    foreach (var game in games) {

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                    }

                } else {

                    games = await _context.Games
                        .OrderBy(g => g.Id)
                        .Include(g => g.User)
                        .ToListAsync();
                }

                gameListTaskResult.Result = true;
                gameListTaskResult.Games = games;

                return gameListTaskResult;

            } catch(Exception) {

                return gameListTaskResult;
            }
        }

        public async Task<GameTaskResult> GetMyGame(int userId, int gameId, bool fullRecord = true) {

            var gameTaskResult = new GameTaskResult() {

                Result = false,
                Game = new Game() {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };

            try {

                var game = new Game();

                if (fullRecord) {

                    game = await _context.Games
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(g => g.User.Id == userId && g.Id == gameId);

                    if (game == null) {

                        game = new Game() {

                            Id = 0,
                            UserId = 0,
                            SudokuMatrixId = 0
                        };

                    } else {

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                        gameTaskResult.Result = true;
                        gameTaskResult.Game = game;
                    }

                } else {

                    game = await _context.Games
                        .FirstOrDefaultAsync(g => g.User.Id == userId && g.Id == gameId);

                    if (game == null) {

                        game = new Game() {

                            Id = 0,
                            UserId = 0,
                            SudokuMatrixId = 0
                        };

                    } else {

                        gameTaskResult.Result = true;
                        gameTaskResult.Game = game;
                    }
                }

                return gameTaskResult;

            } catch(Exception) {

                return gameTaskResult;
            }
        }

        public async Task<GameListTaskResult> GetMyGames(int userId, bool fullRecord = true) {

            var gameListTaskResult = new GameListTaskResult() {

                Result = false,
                Games = new List<Game>()
            };

            try {

                if (fullRecord) {

                    var games = await _context.Games
                        .Where(g => g.User.Id == userId)
                        .OrderBy(g => g.Id)
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .ToListAsync();

                    foreach (var game in games){

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                    }

                    gameListTaskResult.Result = true;
                    gameListTaskResult.Games = games;

                } else {

                    var games = await _context.Games
                        .Where(g => g.User.Id == userId)
                        .OrderBy(g => g.Id)
                        .ToListAsync();

                    gameListTaskResult.Result = true;
                    gameListTaskResult.Games = games;
                }

                return gameListTaskResult;

            } catch(Exception) {

                return gameListTaskResult;
            }
        }

        public async Task<bool> DeleteMyGame(int userId, int gameId) {

            var result = false;

            try {

                var game = await _context.Games
                    .FirstOrDefaultAsync(predicate: g => g.Id == gameId && g.User.Id == userId);

                if (game != null) {

                    _context.Games.Remove(game);
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<GameTaskResult> CheckGame(UpdateGameRO updateGameRO) {

            var gameTaskResult = new GameTaskResult() {

                Result = false,
                Game = new Game() {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };

            try {

                var game = await _context.Games
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                        .FirstOrDefaultAsync(predicate: g => g.Id == updateGameRO.GameId);

                if (game != null) {

                    game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                    game.IsSolved();

                    foreach (var cell in game.SudokuMatrix.SudokuCells) {

                        _context.SudokuCells.Update(cell);
                    }

                    await _context.SaveChangesAsync();

                    gameTaskResult.Result = true;
                    gameTaskResult.Game = game;

                }

                return gameTaskResult;

            } catch(Exception) {

                return gameTaskResult;
            }
        }
    }
}
