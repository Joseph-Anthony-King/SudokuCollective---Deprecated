using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Extensions;

namespace SudokuCollective.Data.Services
{
    public class GamesService : IGamesService
    {
        #region Fields
        private readonly IGamesRepository<Game> gamesRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly IDifficultiesRepository<Difficulty> difficultiesRepository;
        private readonly ISolutionsRepository<SudokuSolution> solutionsRepository;
        #endregion

        #region Constructor
        public GamesService(
            IGamesRepository<Game> gamesRepo,
            IAppsRepository<App> appsRepo,
            IUsersRepository<User> usersRepo, 
            IDifficultiesRepository<Difficulty> difficultiesRepo,
            ISolutionsRepository<SudokuSolution> solutionsRepo)
        {
            gamesRepository = gamesRepo;
            appsRepository = appsRepo;
            usersRepository = usersRepo;
            difficultiesRepository = difficultiesRepo;
            solutionsRepository = solutionsRepo;
        }
        #endregion

        #region Methods
        public async Task<IGameResult> CreateGame(
            ICreateGameRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new GameResult();

            try
            {
                var userResponse = await usersRepository.GetById(request.UserId);

                if (userResponse.Success)
                {
                    var difficultyResponse = await difficultiesRepository.GetById(request.DifficultyId);

                    if (difficultyResponse.Success)
                    {
                        var game = new Game(
                            (User)userResponse.Object,
                            new SudokuMatrix(),
                            (Difficulty)difficultyResponse.Object,
                            request.AppId);

                        game.SudokuMatrix.GenerateSolution();

                        var gameResponse = await gamesRepository.Add(game);

                        if (gameResponse.Success)
                        {
                            ((IGame)gameResponse.Object).User = null;
                            ((IGame)gameResponse.Object).SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                            ((IGame)gameResponse.Object).SudokuMatrix.SudokuCells.OrderBy(cell => cell.Index);

                            result.Success = gameResponse.Success;
                            result.Message = GamesMessages.GameCreatedMessage;
                            result.Game = (IGame)gameResponse.Object;

                            return result;
                        }
                        else if (!gameResponse.Success && gameResponse.Exception != null)
                        {
                            result.Success = gameResponse.Success;
                            result.Message = gameResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = GamesMessages.GameNotCreatedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = DifficultiesMessages.DifficultyDoesNotExistMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserDoesNotExistMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGameResult> UpdateGame(int id, IUpdateGameRequest request)
        {
            var result = new GameResult();

            try
            {
                if (await gamesRepository.HasEntity(id))
                {
                    var gameResponse = await gamesRepository.GetById(id, true);

                    if (gameResponse.Success)
                    {
                        foreach (var cell in request.SudokuCells)
                        {
                            foreach (var savedCell in ((Game)gameResponse.Object).SudokuMatrix.SudokuCells)
                            {
                                if (savedCell.Id == cell.Id && savedCell.Hidden)
                                {
                                    savedCell.DisplayedValue = cell.DisplayedValue;
                                }
                            }
                        }

                        var updateGameResponse = await gamesRepository.Update((Game)gameResponse.Object);

                        if (updateGameResponse.Success)
                        {
                            result.Success = updateGameResponse.Success;
                            result.Message = GamesMessages.GameUpdatedMessage;
                            result.Game = (Game)updateGameResponse.Object;

                            return result;
                        }
                        else if (!updateGameResponse.Success && updateGameResponse.Exception != null)
                        {
                            result.Success = updateGameResponse.Success;
                            result.Message = updateGameResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = GamesMessages.GameNotUpdatedMessage;

                            return result;
                        }
                    }
                    else if (!gameResponse.Success && gameResponse.Exception != null)
                    {
                        result.Success = gameResponse.Success;
                        result.Message = gameResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = GamesMessages.GameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteGame(int id)
        {
            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotFoundMessage;

                return result;
            }

            try
            {
                var gameResponse = await gamesRepository.GetById(id);

                if (gameResponse.Success)
                {
                    var deleteGameResponse = await gamesRepository.Delete((Game)gameResponse.Object);

                    if (deleteGameResponse.Success)
                    {
                        result.Success = deleteGameResponse.Success;
                        result.Message = GamesMessages.GameDeletedMessage;

                        return result;
                    }
                    else if (!deleteGameResponse.Success && deleteGameResponse.Exception != null)
                    {
                        result.Success = deleteGameResponse.Success;
                        result.Message = deleteGameResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotDeletedMessage;

                        return result;
                    }
                }
                else if (!gameResponse.Success && gameResponse.Exception != null)
                {
                    result.Success = gameResponse.Success;
                    result.Message = gameResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = GamesMessages.GameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGameResult> GetGame(int id, int appId, bool fullRecord = true)
        {
            var result = new GameResult();

            if (id == 0 || appId == 0)
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotFoundMessage;
            }

            try
            {
                if (await appsRepository.HasEntity(appId))
                {
                    var gameResponse = await gamesRepository.GetAppGame(id, appId, fullRecord);

                    if (gameResponse.Success)
                    {
                        var game = (Game)gameResponse.Object;

                        if (fullRecord)
                        {
                            foreach (var userRole in game.User.Roles)
                            {
                                userRole.Role.Users = null;
                            }

                            game.User.Apps = null;

                            game.SudokuMatrix.Difficulty.Matrices = null;
                        }
                        else
                        {
                            game.User = null;
                            game.SudokuMatrix = null;
                            game.SudokuSolution = null;
                        }

                        result.Success = true;
                        result.Message = GamesMessages.GameFoundMessage;
                        result.Game = game;

                        return result;
                    }
                    else if (!gameResponse.Success && gameResponse.Exception != null)
                    {
                        result.Success = gameResponse.Success;
                        result.Message = gameResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = true;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGamesResult> GetGames(
            IGamesRequest request, 
            bool fullRecord = true)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    var response = await gamesRepository.GetAppGames(request.AppId, fullRecord);

                    if (response.Success)
                    {
                        if (request.Paginator != null)
                        {
                            if (StaticDataHelpers.IsPageValid(request.Paginator, response.Objects))
                            {
                                if (request.Paginator.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (request.Paginator.SortBy == SortValue.ID)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.Id)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.SCORE)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .Where(g => g.Score != 0 && 
                                                g.Score != int.MaxValue && 
                                                g.Score != 0 &&
                                                !g.ContinueGame)
                                            .OrderByDescending(g => g.Score)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .Where(g => g.Score != 0 &&
                                                g.Score != int.MaxValue &&
                                                g.Score != 0 &&
                                                !g.ContinueGame)
                                            .OrderBy(g => g.Score)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.DATECREATED)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.DateCreated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.DateUpdated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.PageNotFoundMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }

                        if (fullRecord)
                        {
                            foreach (var game in result.Games)
                            {
                                foreach (var userRole in game.User.Roles)
                                {
                                    userRole.Role.Users = null;
                                }

                                game.User.Apps = null;

                                game.SudokuMatrix.Difficulty.Matrices = null;
                            }
                        }
                        else
                        {
                            foreach (var game in result.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix = null;
                                game.SudokuSolution = null;
                            }
                        }

                        result.Success = response.Success;
                        result.Message = GamesMessages.GamesFoundMessage;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GamesNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGameResult> GetMyGame(
            int id, 
            IGamesRequest request, 
            bool fullRecord = true)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new GameResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotFoundMessage;

                return result;
            }

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    var gameResponse = await gamesRepository.GetMyGame(
                        request.UserId, 
                        id,
                        request.AppId, 
                        fullRecord);

                    if (gameResponse.Success)
                    {
                        var game = (Game)gameResponse.Object;

                        if (fullRecord)
                        {
                            foreach (var userRole in game.User.Roles)
                            {
                                userRole.Role.Users = null;
                            }

                            game.User.Apps = null;

                            game.SudokuMatrix.Difficulty.Matrices = null;
                        }
                        else
                        {
                            game.User = null;
                            game.SudokuMatrix = null;
                            game.SudokuSolution = null;
                        }

                        result.Success = true;
                        result.Message = GamesMessages.GameFoundMessage;
                        result.Game = game;

                        return result;
                    }
                    else if (!gameResponse.Success && gameResponse.Exception != null)
                    {
                        result.Success = gameResponse.Success;
                        result.Message = gameResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = true;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGamesResult> GetMyGames(
            IGamesRequest request,
            bool fullRecord = true)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    var response = await gamesRepository.GetMyGames(
                        request.UserId,
                        request.AppId, 
                        fullRecord);

                    if (response.Success)
                    {
                        if (request.Paginator != null)
                        {
                            if (StaticDataHelpers.IsPageValid(request.Paginator, response.Objects))
                            {
                                if (request.Paginator.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (request.Paginator.SortBy == SortValue.ID)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.Id)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.SCORE)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .Where(g => g.Score != 0 &&
                                                g.Score != int.MaxValue &&
                                                g.Score != 0 &&
                                                !g.ContinueGame)
                                            .OrderByDescending(g => g.Score)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .Where(g => g.Score != 0 &&
                                                g.Score != int.MaxValue &&
                                                g.Score != 0 &&
                                                !g.ContinueGame)
                                            .OrderBy(g => g.Score)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.DATECREATED)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.DateCreated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.Paginator.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!request.Paginator.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                    else
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderByDescending(g => g.DateUpdated)
                                            .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                            .Take(request.Paginator.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.PageNotFoundMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }

                        if (fullRecord)
                        {
                            foreach (var game in result.Games)
                            {
                                foreach (var userRole in game.User.Roles)
                                {
                                    userRole.Role.Users = null;
                                }

                                game.User.Apps = null;

                                game.SudokuMatrix.Difficulty.Matrices = null;
                            }
                        }
                        else
                        {
                            foreach (var game in result.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix = null;
                                game.SudokuSolution = null;
                            }
                        }

                        result.Success = response.Success;
                        result.Message = GamesMessages.GamesFoundMessage;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GamesNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteMyGame(int id, IGamesRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotFoundMessage;

                return result;
            }

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    var response = await gamesRepository.DeleteMyGame(
                        request.UserId, 
                        id, 
                        request.AppId);

                    if (response.Success)
                    {
                        result.Success = true;
                        result.Message = GamesMessages.GameDeletedMessage;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotDeletedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = true;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IGameResult> CheckGame(int id, IUpdateGameRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new GameResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotFoundMessage;

                return result;
            }

            try
            {
                var gameResponse = await gamesRepository.GetById(id, true);

                if (gameResponse.Success)
                {
                    foreach (var cell in request.SudokuCells)
                    {
                        foreach (var savedCell in ((Game)gameResponse.Object).SudokuMatrix.SudokuCells)
                        {
                            if (savedCell.Id == cell.Id && savedCell.Hidden)
                            {
                                savedCell.DisplayedValue = cell.DisplayedValue;
                            }
                        }
                    }

                    if (((Game)gameResponse.Object).IsSolved())
                    {
                        result.Message = GamesMessages.GameSolvedMessage;
                    }
                    else
                    {
                        result.Message = GamesMessages.GameNotSolvedMessage;
                    }

                    var updateGameResponse = await gamesRepository.Update((Game)gameResponse.Object);

                    if (updateGameResponse.Success)
                    {
                        result.Success = updateGameResponse.Success;
                        result.Game = (Game)updateGameResponse.Object;

                        return result;
                    }
                    else if (!updateGameResponse.Success && updateGameResponse.Exception != null)
                    {
                        result.Success = updateGameResponse.Success;
                        result.Message = updateGameResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = GamesMessages.GameNotUpdatedMessage;

                        return result;
                    }
                }
                else if (!gameResponse.Success && gameResponse.Exception != null)
                {
                    result.Success = gameResponse.Success;
                    result.Message = gameResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = GamesMessages.GameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAnnonymousGameResult> CreateAnnonymousGame(DifficultyLevel difficultyLevel)
        {
            var result = new AnnonymousGameResult();

            try
            {
                if (await difficultiesRepository.HasDifficultyLevel(difficultyLevel))
                {
                    var game = new Game(new Difficulty { DifficultyLevel = difficultyLevel });

                    game.SudokuMatrix.GenerateSolution();

                    var sudokuMatrix = new List<List<int>>();

                    for (var i = 0; i < 73; i += 9)
                    {
                        result.SudokuMatrix.Add(game.SudokuMatrix.ToDisplayedIntList().GetRange(i, 9));
                    }

                    result.Success = true;
                    result.Message = GamesMessages.GameCreatedMessage;
                }
                else
                {
                    result.Success = false;
                    result.Message = DifficultiesMessages.DifficultyNotFoundMessage;
                }

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> CheckAnnonymousGame(List<int> intList)
        {
            if (intList == null) throw new ArgumentNullException(nameof(intList));

            var result = new BaseResult();

            if (intList.Count != 81 || intList.Contains(0))
            {
                result.Success = false;
                result.Message = GamesMessages.GameNotSolvedMessage;

                return result;
            }

            var game = new Game(
                new Difficulty
                {
                    DifficultyLevel = DifficultyLevel.TEST
                },
                intList);

            result.Success = game.IsSolved();

            if (result.Success)
            {
                // Add solution to the database
                var solutionsResponse = await solutionsRepository.GetSolvedSolutions();

                if (solutionsResponse.Success)
                {
                    var solutionInDB = false;

                    foreach (var solution in solutionsResponse.Objects.ConvertAll(s => (SudokuSolution)s))
                    {
                        if (solution.SolutionList.IsThisListEqual(game.SudokuSolution.SolutionList))
                        {
                            solutionInDB = true;
                        }
                    }

                    if (!solutionInDB)
                    {
                        _ = solutionsRepository.Create(game.SudokuSolution);
                    }
                }

                result.Message = GamesMessages.GameSolvedMessage;
            }
            else
            {
                result.Message = GamesMessages.GameNotSolvedMessage;
            }

            return result;
        }
        #endregion
    }
}
