using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Interfaces;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class GamesService : IGamesService {

        private readonly ApplicationDbContext _context;
        private readonly IUsersService _userService;
        private readonly IDifficultiesService _difficultiesService;
        private readonly IAppsService _appsService;

        public GamesService(
            ApplicationDbContext context,
            IUsersService usersService,
            IDifficultiesService difficultiesService,
            IAppsService appsService) {

            _context = context;
            _userService = usersService;
            _difficultiesService = difficultiesService;
            _appsService = appsService;
        }

        public async Task<GameTaskResult> CreateGame(
            CreateGameRO createGameRO, bool fullRecord = true) {

            var gameTaskResult = new GameTaskResult();

            try {

                User user;
                Difficulty difficulty;

                var userResult = await _userService.GetUser(createGameRO.UserId);
                var difficultyResult =
                    await _difficultiesService.GetDifficulty(createGameRO.DifficultyId);

                if (userResult.Success && difficultyResult.Success) {

                    user = userResult.User;
                    difficulty = difficultyResult.Difficulty;

                    var userApps = user.Apps;
                    var userRoles = user.Roles;
                    var userGames = user.Games;

                    SudokuMatrix matrix = new SudokuMatrix();
                    matrix.GenerateSolution();

                    var game = new Game(
                        user,
                        matrix,
                        difficulty);

                    _context.ChangeTracker.TrackGraph(user, 
                        e => {

                            if (e.Entry.Entity is User) {

                                e.Entry.State = EntityState.Unchanged;
                                
                            } else if (e.Entry.Entity is Difficulty) {

                                e.Entry.State = EntityState.Unchanged;

                            } else {

                                var dbEntry = (ISudokuOject)e.Entry.Entity;

                                if (dbEntry.Id != 0) {

                                    e.Entry.State = EntityState.Modified;

                                } else {

                                    e.Entry.State = EntityState.Added;
                                }
                            }
                        });

                    foreach (var userApp in userApps) {

                        _context.Entry<UserApp>(userApp).State = EntityState.Unchanged;
                    }

                    foreach (var userRole in userRoles) {

                        _context.Entry<UserRole>(userRole).State = EntityState.Unchanged;
                    }

                    foreach (var userGame in userGames) {

                        if (userGame.Id != 0) {

                            _context.Entry<Game>(userGame).State = EntityState.Unchanged;

                        } else {

                            _context.Entry<Game>(userGame).State = EntityState.Added;
                        }
                    }

                    await _context.SaveChangesAsync();

                    gameTaskResult.Success = true;

                    if (fullRecord) {

                        gameTaskResult.Game = game;

                    } else {

                        game.User.Games = null;
                        game.User.Roles = null;
                        game.User.Apps = null;
                        game.SudokuMatrix.Difficulty.Matrices = null;
                        gameTaskResult.Game = game;
                    }

                } else {

                    gameTaskResult.Message = "Could not find the user or difficulty in the database";
                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<GameTaskResult> UpdateGame(int id, UpdateGameRO updateGameRO) {

            var gameTaskResult = new GameTaskResult();

            try {

                if (id == updateGameRO.GameId) {

                    var game = await _context.Games
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                            .FirstOrDefaultAsync(predicate: g => g.Id == updateGameRO.GameId);

                    if (game == null) {

                        gameTaskResult.Message = "Game not found";

                        return gameTaskResult;
                    }

                    game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                    game.IsSolved();

                    foreach (var cell in game.SudokuMatrix.SudokuCells) {

                        _context.SudokuCells.Update(cell);
                    }

                    _context.ChangeTracker.TrackGraph(game,
                        e => {

                            var dbEntry = (ISudokuOject)e.Entry.Entity;

                            if (dbEntry.Id != 0) {

                                e.Entry.State = EntityState.Modified;

                            } else {

                                e.Entry.State = EntityState.Added;
                            }
                        });

                    await _context.SaveChangesAsync();

                    gameTaskResult.Success = true;
                    gameTaskResult.Game = game;

                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<BaseTaskResult> DeleteGame(int id) {

            var baseTaskResult = new BaseTaskResult();

            try {

                var game = await _context.Games
                    .Include(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: g => g.Id == id);

                if (game == null) {

                    baseTaskResult.Message = "Game not found";

                    return baseTaskResult;
                }

                game.SudokuMatrix = await StaticApiHelpers
                    .AttachSudokuMatrix(game, _context);

                if (game.ContinueGame) {

                    var solution = await _context.SudokuSolutions
                        .FirstOrDefaultAsync(predicate: s => s.Id == game.SudokuSolutionId);

                    _context.SudokuSolutions.Remove(solution);
                }

                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<GameTaskResult> GetGame(int id) {

            var gameTaskResult = new GameTaskResult();

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
                    gameTaskResult.Success = true;
                    gameTaskResult.Game = game;
                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<GameListTaskResult> GetGames(bool fullRecord = true) {

            var gameListTaskResult = new GameListTaskResult();

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

                gameListTaskResult.Success = true;
                gameListTaskResult.Games = games;

                return gameListTaskResult;

            } catch (Exception e) {

                gameListTaskResult.Message = e.Message;

                return gameListTaskResult;
            }
        }

        public async Task<GameTaskResult> GetMyGame(int userId, int gameId, bool fullRecord = true) {

            var gameTaskResult = new GameTaskResult();

            try {

                var game = new Game();

                if (fullRecord) {

                    game = await _context.Games
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(g => g.User.Id == userId && g.Id == gameId);

                    if (game == null) {

                        gameTaskResult.Message = "Game not found";

                        return gameTaskResult;

                    } else {

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                        gameTaskResult.Success = true;
                        gameTaskResult.Game = game;
                    }

                } else {

                    game = await _context.Games
                        .FirstOrDefaultAsync(g => g.User.Id == userId && g.Id == gameId);

                    if (game == null) {

                        gameTaskResult.Message = "Game not found";

                        return gameTaskResult;

                    } else {

                        gameTaskResult.Success = true;
                        gameTaskResult.Game = game;
                    }
                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<GameListTaskResult> GetMyGames(int userId, bool fullRecord = true) {

            var gameListTaskResult = new GameListTaskResult();

            try {

                if (fullRecord) {

                    var games = await _context.Games
                        .OrderBy(g => g.Id)
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .Where(g => g.User.Id == userId)
                        .ToListAsync();

                    foreach (var game in games){

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                    }

                    gameListTaskResult.Success = true;
                    gameListTaskResult.Games = games;

                } else {

                    var games = await _context.Games
                        .OrderBy(g => g.Id)
                        .Where(g => g.User.Id == userId)
                        .ToListAsync();

                    gameListTaskResult.Success = true;
                    gameListTaskResult.Games = games;
                }

                return gameListTaskResult;

            } catch (Exception e) {

                gameListTaskResult.Message = e.Message;

                return gameListTaskResult;
            }
        }

        public async Task<BaseTaskResult> DeleteMyGame(int userId, int gameId) {

            var baseTaskResult = new BaseTaskResult();

            try {

                var game = await _context.Games
                    .Include(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: g => g.Id == gameId && g.User.Id == userId);

                if (game == null) {

                    baseTaskResult.Message = "Game not found";

                    return baseTaskResult;
                }

                game.SudokuMatrix = await StaticApiHelpers
                    .AttachSudokuMatrix(game, _context);

                if (game.ContinueGame) {

                    var solution = await _context.SudokuSolutions
                        .FirstOrDefaultAsync(predicate: s => s.Id == game.SudokuSolutionId);

                    _context.SudokuSolutions.Remove(solution);
                }

                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<GameTaskResult> CheckGame(int id, UpdateGameRO updateGameRO) {

            var gameTaskResult = new GameTaskResult();

            try {

                var game = await _context.Games
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);

                if (game == null) {

                    gameTaskResult.Message = "Game not found";

                    return gameTaskResult;
                }

                game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                game.IsSolved();

                foreach (var cell in game.SudokuMatrix.SudokuCells) {

                    _context.SudokuCells.Update(cell);
                }

                _context.ChangeTracker.TrackGraph(game,
                    e => {

                        var dbEntry = (ISudokuOject)e.Entry.Entity;

                        if (dbEntry.Id != 0) {

                            e.Entry.State = EntityState.Modified;

                        } else {

                            e.Entry.State = EntityState.Added;
                        }
                    });

                await _context.SaveChangesAsync();

                gameTaskResult.Success = true;
                gameTaskResult.Game = game;

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }
    }
}
