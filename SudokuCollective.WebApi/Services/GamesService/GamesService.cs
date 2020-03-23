using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Interfaces;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class GamesService : IGamesService {

        private readonly DatabaseContext _context;

        public GamesService(DatabaseContext context) {

            _context = context;
        }

        public async Task<GameResult> CreateGame(
            CreateGameRequest createGameRequest, bool fullRecord = false) {

            var gameTaskResult = new GameResult();

            try {
                    
                var difficulty = await _context.Difficulties
                    .FirstOrDefaultAsync(d => d.Id == createGameRequest.DifficultyId);

                var user = await _context.Users
                    .Include(u => u.Apps)
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == createGameRequest.UserId);
                    
                foreach (var userRole in user.Roles) {

                    userRole.Role = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
                }

                user.updateRoles();

                SudokuMatrix matrix = new SudokuMatrix();
                matrix.GenerateSolution();

                var game = new Game(
                    user,
                    matrix,
                    difficulty,
                    createGameRequest.AppId);

                _context.ChangeTracker.TrackGraph(user, 
                    e => {

                        var dbEntry = (IEntityBase)e.Entry.Entity;

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

        public async Task<GameResult> UpdateGame(int id, UpdateGameRequest updateGameRequest) {

            var gameTaskResult = new GameResult();

            try {

                if (id == updateGameRequest.GameId && GamesServiceUtilities
                        .EnsureSudokuCellsAreAttachedToThisGame(
                            id, 
                            updateGameRequest.SudokuCells, 
                            _context)) {

                    var game = await _context.Games
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                            .FirstOrDefaultAsync(predicate: g => g.Id == updateGameRequest.GameId);

                    if (game == null) {

                        gameTaskResult.Message = "Game not found";

                        return gameTaskResult;
                    }

                    await game.SudokuMatrix.AttachSudokuCells(_context);

                    game.SudokuMatrix.SudokuCells = game.SudokuMatrix
                        .SudokuCells
                        .OrderBy(cell => cell.Index)
                        .ToList();

                    int index = 1;

                    foreach (var cell in game.SudokuMatrix.SudokuCells) {

                        if (cell.DisplayValue 
                            != updateGameRequest.SudokuCells
                                .Where(c => c.Index == index)
                                .Select(c => c.DisplayValue)
                                .FirstOrDefault()) {

                            cell.DisplayValue = updateGameRequest.SudokuCells
                                .Where(c => c.Index == index)
                                .Select(c => c.DisplayValue)
                                .FirstOrDefault();
                        }

                        _context.SudokuCells.Update(cell);

                        index++;
                    }

                    game.IsSolved();

                    _context.ChangeTracker.TrackGraph(game,
                        e => {

                            var dbEntry = (IEntityBase)e.Entry.Entity;

                            if (dbEntry.Id != 0) {

                                e.Entry.State = EntityState.Modified;

                            } else {

                                e.Entry.State = EntityState.Added;
                            }
                        });

                    await _context.SaveChangesAsync();

                    gameTaskResult.Success = true;
                    gameTaskResult.Game = game;

                } else {

                    gameTaskResult.Message = "The game id or cells were incorrect";
                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<BaseResult> DeleteGame(int id) {

            var baseTaskResult = new BaseResult();

            try {

                var game = await _context.Games
                    .Include(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: g => g.Id == id);

                if (game == null) {

                    baseTaskResult.Message = "Game not found";

                    return baseTaskResult;
                }

                await game.SudokuMatrix.AttachSudokuCells(_context);

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

        public async Task<GameResult> GetGame(int id) {

            var gameTaskResult = new GameResult();

            try {

                var game = await _context.Games
                    .Include(g => g.User).ThenInclude(u => u.Roles)
                    .Include(g => g.SudokuMatrix)
                    .Include(g => g.SudokuSolution)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null) {

                    gameTaskResult.Message = "Game not found";

                    return gameTaskResult;

                } else {

                    await game.SudokuMatrix.AttachSudokuCells(_context);
                    gameTaskResult.Success = true;
                    gameTaskResult.Game = game;
                }

                return gameTaskResult;

            } catch (Exception e) {

                gameTaskResult.Message = e.Message;

                return gameTaskResult;
            }
        }

        public async Task<GamesResult> GetGames(
            BaseRequest baseRequest, bool fullRecord = false) {

            var gameListTaskResult = new GamesResult();

            try {

                var games = new List<Game>();

                if (fullRecord) {

                    games = await GamesServiceUtilities
                        .RetrieveGames(baseRequest.PageListModel, _context);

                    foreach (var game in games) {

                        await game.SudokuMatrix.AttachSudokuCells(_context);
                    }

                } else {

                    games = await GamesServiceUtilities
                        .RetrieveGames(baseRequest.PageListModel, _context);
                }

                gameListTaskResult.Success = true;
                gameListTaskResult.Games = games;

                return gameListTaskResult;

            } catch (Exception e) {

                gameListTaskResult.Message = e.Message;

                return gameListTaskResult;
            }
        }

        public async Task<GameResult> GetMyGame(int userId, int gameId, bool fullRecord = false) {

            var gameTaskResult = new GameResult();

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

                        await game.SudokuMatrix.AttachSudokuCells(_context);
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

        public async Task<GamesResult> GetMyGames(
            int userId,
            GetMyGameRequest getMyGameRequest, 
            bool fullRecord = false) {

            var gameListTaskResult = new GamesResult();

            try {

                if (fullRecord) {

                    var games = await GamesServiceUtilities
                        .RetrieveGames(getMyGameRequest.PageListModel, _context, userId);

                    foreach (var game in games){

                        await game.SudokuMatrix.AttachSudokuCells(_context);
                    }

                    gameListTaskResult.Success = true;
                    gameListTaskResult.Games = games;

                } else {

                    var games = await GamesServiceUtilities
                        .RetrieveGames(getMyGameRequest.PageListModel, _context, userId);

                    gameListTaskResult.Success = true;
                    gameListTaskResult.Games = games;
                }

                return gameListTaskResult;

            } catch (Exception e) {

                gameListTaskResult.Message = e.Message;

                return gameListTaskResult;
            }
        }

        public async Task<BaseResult> DeleteMyGame(int userId, int gameId) {

            var baseTaskResult = new BaseResult();

            try {

                var game = await _context.Games
                    .Include(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: g => g.Id == gameId && g.User.Id == userId);

                if (game == null) {

                    baseTaskResult.Message = "Game not found";

                    return baseTaskResult;
                }

                await game.SudokuMatrix.AttachSudokuCells(_context);

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

        public async Task<GameResult> CheckGame(int id, UpdateGameRequest updateGameRequest) {

            var gameTaskResult = new GameResult();

            try {

                var game = await _context.Games
                        .Include(g => g.User).ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix).ThenInclude(m => m.Difficulty)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);

                if (game == null) {

                    gameTaskResult.Message = "Game not found";

                    return gameTaskResult;
                }

                game.SudokuMatrix.SudokuCells = _context.SudokuCells
                    .Where(cell => cell.SudokuMatrixId == game.SudokuMatrixId)
                    .OrderBy(cell => cell.Index)
                    .ToList();

                int index = 1;

                foreach (var cell in game.SudokuMatrix.SudokuCells)
                {

                    if (cell.DisplayValue
                        != updateGameRequest.SudokuCells
                            .Where(c => c.Index == index)
                            .Select(c => c.DisplayValue)
                            .FirstOrDefault())
                    {

                        cell.DisplayValue = updateGameRequest.SudokuCells
                            .Where(c => c.Index == index)
                            .Select(c => c.DisplayValue)
                            .FirstOrDefault();
                    }

                    _context.SudokuCells.Update(cell);

                    index++;
                }

                game.IsSolved();

                _context.ChangeTracker.TrackGraph(game,
                    e => {

                        var dbEntry = (IEntityBase)e.Entry.Entity;

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
