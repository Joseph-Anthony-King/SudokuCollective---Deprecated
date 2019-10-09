using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Interfaces;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.TaskModels;
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
            IDifficultiesService difficultiesService,
            IAppsService appsService) {

            _context = context;
            _userService = usersService;
            _difficultiesService = difficultiesService;
        }

        public async Task<GameTaskResult> CreateGame(
            CreateGameRO createGameRO, bool fullRecord = false) {

            var gameTaskResult = new GameTaskResult();

            try {
                    
                var difficulty = await _context.Difficulties
                    .FirstOrDefaultAsync(d => d.Id == createGameRO.DifficultyId);

                var user = await _context.Users
                    .Include(u => u.Apps)
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == createGameRO.UserId);
                    
                foreach (var userRole in user.Roles) {

                    userRole.Role = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
                }

                SudokuMatrix matrix = new SudokuMatrix();
                matrix.GenerateSolution();

                var game = new Game(
                    user,
                    matrix,
                    difficulty);

                _context.ChangeTracker.TrackGraph(user, 
                    e => {

                        var dbEntry = (IDBEntry)e.Entry.Entity;

                        if (dbEntry.Id != 0) {

                            e.Entry.State = EntityState.Modified;

                        } else {

                            e.Entry.State = EntityState.Added;
                        }
                    });

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

                        gameTaskResult.Message = "Game Not Found";

                        return gameTaskResult;
                    }

                    game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                    game.IsSolved();

                    foreach (var cell in game.SudokuMatrix.SudokuCells) {

                        _context.SudokuCells.Update(cell);
                    }

                    _context.ChangeTracker.TrackGraph(game,
                        e => {

                            var dbEntry = (IDBEntry)e.Entry.Entity;

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

                    baseTaskResult.Message = "Game Not Found";

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

                    gameTaskResult.Message = "Game Not Found";

                    return gameTaskResult;

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

        public async Task<GameListTaskResult> GetGames(
            BaseRequestRO baseRequestRO, bool fullRecord = false) {

            var gameListTaskResult = new GameListTaskResult();

            try {

                var games = new List<Game>();

                if (fullRecord) {

                    games = await GamesServiceUtilities
                        .RetrieveGames(baseRequestRO.PageListModel, _context);

                    foreach (var game in games) {

                        game.SudokuMatrix = await StaticApiHelpers
                            .AttachSudokuMatrix(game, _context);
                    }

                } else {

                    games = await GamesServiceUtilities
                        .RetrieveGames(baseRequestRO.PageListModel, _context);
                }

                gameListTaskResult.Success = true;
                gameListTaskResult.Games = games;

                return gameListTaskResult;

            } catch (Exception e) {

                gameListTaskResult.Message = e.Message;

                return gameListTaskResult;
            }
        }

        public async Task<GameTaskResult> GetMyGame(int userId, int gameId, bool fullRecord = false) {

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

                        gameTaskResult.Message = "Game Not Found";

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

                        gameTaskResult.Message = "Game Not Found";

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

        public async Task<GameListTaskResult> GetMyGames(
            int userId,
            GetMyGameRO getMyGameRO, 
            bool fullRecord = false) {

            var gameListTaskResult = new GameListTaskResult();

            try {

                if (fullRecord) {

                    var games = await GamesServiceUtilities
                        .RetrieveGames(getMyGameRO.PageListModel, _context, userId);

                    foreach (var game in games){

                        game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                    }

                    gameListTaskResult.Success = true;
                    gameListTaskResult.Games = games;

                } else {

                    var games = await GamesServiceUtilities
                        .RetrieveGames(getMyGameRO.PageListModel, _context, userId);

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

                    baseTaskResult.Message = "Game Not Found";

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

                    gameTaskResult.Message = "Game Not Found";

                    return gameTaskResult;
                }

                game.SudokuMatrix.SudokuCells = updateGameRO.SudokuCells;

                game.IsSolved();

                foreach (var cell in game.SudokuMatrix.SudokuCells) {

                    _context.SudokuCells.Update(cell);
                }

                _context.ChangeTracker.TrackGraph(game,
                    e => {

                        var dbEntry = (IDBEntry)e.Entry.Entity;

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
