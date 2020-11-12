using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Helpers;

namespace SudokuCollective.Data.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository<Game> gamesRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly IDifficultiesRepository<Difficulty> difficultiesRepository;
        private readonly string gameNotFoundMessage;
        private readonly string unableToCreateGameMessage;
        private readonly string unableToDeleteGameMessage;
        private readonly string unableToGetGamesMessage;
        private readonly string appNotFoundMessage;
        private readonly string userDoesNotExistMessage;
        private readonly string difficultyDoesNotExistMessage;
        private readonly string pageNotFoundMessage;
        private readonly string sortValueNotImplementedMessage;

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
            gameNotFoundMessage = "Game not found";
            unableToCreateGameMessage = "Unable to create game";
            unableToDeleteGameMessage = "Unable to delete game";
            unableToGetGamesMessage = "Unable to get games";
            appNotFoundMessage = "App not found";
            userDoesNotExistMessage = "User does not exist";
            difficultyDoesNotExistMessage = "Difficulty does not exist";
            pageNotFoundMessage = "Page not found";
            sortValueNotImplementedMessage = "Sorting not implemented for this sort value";
        }

        public async Task<IGameResult> CreateGame(
            ICreateGameRequest createGameRequest, bool fullRecord = true)
        {
            var result = new GameResult();

            try
            {
                if (await usersRepository.HasEntity(createGameRequest.UserId))
                {
                    if (await difficultiesRepository.HasEntity(createGameRequest.DifficultyId))
                    {
                        var userResponse = await usersRepository.GetById(createGameRequest.UserId, true);

                        if (userResponse.Success)
                        {
                            var difficultyResponse = await difficultiesRepository.GetById(createGameRequest.DifficultyId);

                            if (difficultyResponse.Success)
                            {
                                var game = new Game(
                                    (User)userResponse.Object,
                                    new SudokuMatrix(),
                                    (Difficulty)difficultyResponse.Object,
                                    createGameRequest.AppId);

                                game.SudokuMatrix.GenerateSolution();

                                var gameResponse = await gamesRepository.Create(game);

                                if (gameResponse.Success)
                                {
                                    ((IGame)gameResponse.Object).User = null;
                                    ((IGame)gameResponse.Object).SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                                    ((IGame)gameResponse.Object).SudokuMatrix.SudokuCells.OrderBy(cell => cell.Index);

                                    result.Success = gameResponse.Success;
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
                                    result.Message = unableToCreateGameMessage;

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
                                result.Message = difficultyDoesNotExistMessage;

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
                            result.Message = userDoesNotExistMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = difficultyDoesNotExistMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = userDoesNotExistMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IGameResult> UpdateGame(int id, IUpdateGameRequest updateGameRequest)
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
                                if (savedCell.Id == cell.Id && savedCell.Obscured)
                                {
                                    savedCell.DisplayValue = cell.DisplayValue;
                                }
                            }
                        }

                        var updateGameResponse = await gamesRepository.Update((Game)gameResponse.Object);

                        if (updateGameResponse.Success)
                        {
                            result.Success = updateGameResponse.Success;
                            result.Message = "Game Updated";
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
                            result.Message = unableToCreateGameMessage;

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
                        result.Message = gameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

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
                            result.Message = unableToDeleteGameMessage;

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
                        result.Message = gameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IGameResult> GetGame(int id, int appId, bool fullRecord = true)
        {
            var result = new GameResult();

            try
            {
                if (await gamesRepository.HasEntity(id))
                {
                    if (await appsRepository.HasEntity(appId))
                    {
                        var gameResponse = await gamesRepository.GetGame(id, appId, fullRecord);

                        if (gameResponse.Success)
                        {
                            ((IGame)gameResponse.Object).User = null;
                            if (((IGame)gameResponse.Object).SudokuMatrix.Difficulty != null)
                            {
                                ((IGame)gameResponse.Object).SudokuMatrix.Difficulty.Matrices = null;
                            }
                            ((IGame)gameResponse.Object).SudokuMatrix.SudokuCells.OrderBy(cell => cell.Index);

                            result.Success = true;
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
                            result.Message = gameNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = appNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = true;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IGamesResult> GetGames(
            IGetGamesRequest getGamesRequest, 
            bool fullRecord = true)
        {
            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(getGamesRequest.AppId))
                {
                    var response = await gamesRepository.GetGames(getGamesRequest.AppId, fullRecord);

                    if (response.Success)
                    {
                        if (getGamesRequest.PageListModel != null)
                        {
                            if (StaticApiHelpers.IsPageValid(getGamesRequest.PageListModel, response.Objects))
                            {
                                if (getGamesRequest.PageListModel.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.ID)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.DATECREATED)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = sortValueNotImplementedMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = pageNotFoundMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }

                        result.Success = response.Success;

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
                        result.Message = unableToGetGamesMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = appNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IGameResult> GetMyGame(
            int gameid, 
            IGetGamesRequest getMyGameRequest, 
            bool fullRecord = true)
        {
            var result = new GameResult();

            try
            {
                if (await gamesRepository.HasEntity(gameid))
                {
                    if (await appsRepository.HasEntity(getMyGameRequest.AppId))
                    {
                        var gameResponse = await gamesRepository.GetMyGame(
                            getMyGameRequest.UserId, 
                            gameid, 
                            getMyGameRequest.AppId, 
                            fullRecord);

                        if (gameResponse.Success)
                        {
                            ((IGame)gameResponse.Object).User = null;
                            if (((IGame)gameResponse.Object).SudokuMatrix.Difficulty != null)
                            {
                                ((IGame)gameResponse.Object).SudokuMatrix.Difficulty.Matrices = null;
                            }
                            ((IGame)gameResponse.Object).SudokuMatrix.SudokuCells.OrderBy(cell => cell.Index);

                            result.Success = true;
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
                            result.Message = gameNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = appNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = true;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IGamesResult> GetMyGames(
            IGetGamesRequest getGamesRequest,
            bool fullRecord = true)
        {
            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(getGamesRequest.AppId))
                {
                    var response = await gamesRepository.GetMyGames(
                        getGamesRequest.UserId, 
                        getGamesRequest.AppId, 
                        fullRecord);

                    if (response.Success)
                    {
                        if (getGamesRequest.PageListModel != null)
                        {
                            if (StaticApiHelpers.IsPageValid(getGamesRequest.PageListModel, response.Objects))
                            {
                                if (getGamesRequest.PageListModel.SortBy == SortValue.NULL)
                                {
                                    result.Games = response.Objects.ConvertAll(g => (IGame)g);
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.ID)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.Id)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.DATECREATED)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateCreated)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else if (getGamesRequest.PageListModel.SortBy == SortValue.DATEUPDATED)
                                {
                                    if (!getGamesRequest.PageListModel.OrderByDescending)
                                    {
                                        foreach (var obj in response.Objects)
                                        {
                                            result.Games.Add((IGame)obj);
                                        }

                                        result.Games = result.Games
                                            .OrderBy(g => g.DateUpdated)
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
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
                                            .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                            .Take(getGamesRequest.PageListModel.ItemsPerPage)
                                            .ToList();
                                    }
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = sortValueNotImplementedMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = pageNotFoundMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }

                        result.Success = response.Success;

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
                        result.Message = unableToGetGamesMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = appNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteMyGame(int gameid, IGetGamesRequest getMyGameRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await gamesRepository.HasEntity(gameid))
                {
                    if (await appsRepository.HasEntity(getMyGameRequest.AppId))
                    {
                        var gameResponse = await gamesRepository.DeleteMyGame(
                            getMyGameRequest.UserId, 
                            gameid, 
                            getMyGameRequest.AppId);

                        if (gameResponse.Success)
                        {
                            result.Success = true;

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
                            result.Message = gameNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = appNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = true;
                result.Message = e.Message;

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
                                if (savedCell.Id == cell.Id && savedCell.Obscured)
                                {
                                    savedCell.DisplayValue = cell.DisplayValue;
                                }
                            }
                        }

                        if (((Game)gameResponse.Object).IsSolved())
                        {
                            result.Message = "Game solved";
                        }
                        else
                        {
                            result.Message = "Game unsolved";
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
                            result.Message = unableToCreateGameMessage;

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
                        result.Message = gameNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = gameNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }
    }
}
