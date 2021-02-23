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

namespace SudokuCollective.Data.Services
{
    public class GamesService : IGamesService
    {
        #region Fields
        private readonly IGamesRepository<Game> gamesRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly IDifficultiesRepository<Difficulty> difficultiesRepository;
        #endregion

        #region Constructor
        public GamesService(
            IGamesRepository<Game> gamesRepo,
            IAppsRepository<App> appsRepo,
            IUsersRepository<User> usersRepo, 
            IDifficultiesRepository<Difficulty> difficultiesRepo)
        {
            gamesRepository = gamesRepo;
            appsRepository = appsRepo;
            usersRepository = usersRepo;
            difficultiesRepository = difficultiesRepo;
        }
        #endregion

        #region Methods
        public async Task<IGameResult> CreateGame(
            ICreateGameRequest request)
        {
            var result = new GameResult();

            try
            {
                if (await usersRepository.HasEntity(request.UserId))
                {
                    if (await difficultiesRepository.HasEntity(request.DifficultyId))
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
                            else if (!difficultyResponse.Success && difficultyResponse.Exception != null)
                            {
                                result.Success = difficultyResponse.Success;
                                result.Message = difficultyResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = DifficultiesMessages.DifficultyDoesNotExistMessage;

                                return result;
                            }
                        }
                        else if (!userResponse.Success && userResponse.Exception != null)
                        {
                            result.Success = userResponse.Success;
                            result.Message = userResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserDoesNotExistMessage;

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

            try
            {
                if (await gamesRepository.HasEntity(id))
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

            try
            {
                if (await appsRepository.HasEntity(appId))
                {
                    if (await gamesRepository.HasEntity(id))
                    {
                        var gameResponse = await gamesRepository.GetAppGame(id, appId, fullRecord);

                        if (gameResponse.Success)
                        {
                            var game = (Game)gameResponse.Object;

                            if (fullRecord)
                            {
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
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
            IGetGamesRequest request, 
            bool fullRecord = true)
        {
            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    var response = await gamesRepository.GetAppGames(request.AppId, fullRecord);

                    if (response.Success)
                    {
                        if (request.PageListModel != null)
                        {
                            if (StaticDataHelpers.IsPageValid(request.PageListModel, response.Objects))
                            {
                                if (request.PageListModel.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (request.PageListModel.SortBy == SortValue.ID)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.SCORE)
                                {
                                    if (!request.PageListModel.OrderByDescending)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.DATECREATED)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
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
            int gameid, 
            IGetGamesRequest request, 
            bool fullRecord = true)
        {
            var result = new GameResult();

            try
            {
                if (await appsRepository.HasEntity(request.AppId))
                {
                    if (await gamesRepository.HasEntity(gameid))
                    {
                        var gameResponse = await gamesRepository.GetMyGame(
                            request.UserId, 
                            gameid,
                            request.AppId, 
                            fullRecord);

                        if (gameResponse.Success)
                        {
                            var game = (Game)gameResponse.Object;

                            if (fullRecord)
                            {
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
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
            IGetGamesRequest request,
            bool fullRecord = true)
        {
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
                        if (request.PageListModel != null)
                        {
                            if (StaticDataHelpers.IsPageValid(request.PageListModel, response.Objects))
                            {
                                if (request.PageListModel.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (request.PageListModel.SortBy == SortValue.ID)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.SCORE)
                                {
                                    if (!request.PageListModel.OrderByDescending)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.DATECREATED)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (request.PageListModel.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!request.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                            .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                            .Take(request.PageListModel.ItemsPerPage)
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
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
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

        public async Task<IBaseResult> DeleteMyGame(int gameid, IGetGamesRequest getMyGameRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(getMyGameRequest.AppId))
                {
                    if (await gamesRepository.HasEntity(gameid))
                    {
                        var response = await gamesRepository.DeleteMyGame(
                            getMyGameRequest.UserId, 
                            gameid, 
                            getMyGameRequest.AppId);

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

        public async Task<IGameResult> CheckGame(int id, IUpdateGameRequest updateGameRequest)
        {
            var result = new GameResult();

            try
            {
                if (await gamesRepository.HasEntity(id))
                {
                    var gameResponse = await gamesRepository.GetById(id, true);

                    if (gameResponse.Success)
                    {
                        foreach (var cell in updateGameRequest.SudokuCells)
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
        #endregion
    }
}
