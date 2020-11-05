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
        private readonly string unableToUpdateGameMessage;
        private readonly string unableToDeleteGameMessage;
        private readonly string unableToGetGamesMessage;
        private readonly string appNotFoundMessage;
        private readonly string userDoesNotExistMessage;
        private readonly string difficultyDoesNotExistMessage;
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
            unableToUpdateGameMessage = "Unable to update game";
            unableToDeleteGameMessage = "Unable to delete game";
            unableToGetGamesMessage = "Unable to get games";
            appNotFoundMessage = "App not found";
            userDoesNotExistMessage = "User does not exist";
            difficultyDoesNotExistMessage = "Difficulty does not exist";
            sortValueNotImplementedMessage = "Sorting not implemented for this sort value";
        }

        public async Task<IGameResult> CreateGame(
            ICreateGameRequest createGameRequest, bool fullRecord = false)
        {
            var result = new GameResult();

            try
            {
                if (await usersRepository.HasEntity(createGameRequest.UserId))
                {
                    if (await difficultiesRepository.HasEntity(createGameRequest.DifficultyId))
                    {
                        var userResponse = await usersRepository.GetById(createGameRequest.UserId);

                        if (userResponse.Success)
                        {
                            var difficultyResponse = await difficultiesRepository.GetById(createGameRequest.DifficultyId);

                            if (difficultyResponse.Success)
                            {
                                var matrix = new SudokuMatrix();

                                matrix.GenerateSolution();

                                var game = new Game(
                                    (User)userResponse.Object,
                                    matrix,
                                    (Difficulty)difficultyResponse.Object,
                                    createGameRequest.AppId);

                                var gameResponse = await gamesRepository.Create(game);

                                if (gameResponse.Success)
                                {
                                    result.Success = gameResponse.Success;

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
                    var gameResponse = await gamesRepository.GetById(id);

                    if (gameResponse.Success)
                    {
                        ((Game)gameResponse.Object).SudokuMatrix.SudokuCells = updateGameRequest.SudokuCells.ConvertAll(c => (SudokuCell)c);

                        var updateGameResponse = await gamesRepository.Update((Game)gameResponse.Object);

                        if (updateGameResponse.Success)
                        {
                            result.Success = updateGameResponse.Success;

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
            var result = new GameResult();

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

        public async Task<IGameResult> GetGame(int id, int appId, bool fullRecord = false)
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
                            result.Success = true;
                            result.Game = (Game)gameResponse.Object;

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
            bool fullRecord = false)
        {
            var result = new GamesResult();

            try
            {
                if (await appsRepository.HasEntity(getGamesRequest.AppId))
                {
                    var response = await gamesRepository.GetGames(getGamesRequest.AppId, fullRecord);

                    if (response.Success)
                    {
                        if (getGamesRequest.PageListModel.SortBy == SortValue.NULL)
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }
                        else if (getGamesRequest.PageListModel.SortBy == SortValue.ID)
                        {
                            if (!getGamesRequest.PageListModel.OrderByDescending)
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderBy(g => g.Id)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderByDescending(g => g.Id)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                        }
                        else if (getGamesRequest.PageListModel.SortBy == SortValue.DATECREATED)
                        {
                            if (!getGamesRequest.PageListModel.OrderByDescending)
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderBy(g => g.DateCreated)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderByDescending(g => g.DateCreated)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = sortValueNotImplementedMessage;

                            return result;
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
            bool fullRecord = false)
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
                            result.Success = true;
                            result.Game = (Game)gameResponse.Object;

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
            bool fullRecord = false)
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
                        if (getGamesRequest.PageListModel.SortBy == SortValue.NULL)
                        {
                            result.Games = response.Objects.ConvertAll(g => (IGame)g);
                        }
                        else if (getGamesRequest.PageListModel.SortBy == SortValue.ID)
                        {
                            if (!getGamesRequest.PageListModel.OrderByDescending)
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderBy(a => a.Id)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderByDescending(a => a.Id)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                        }
                        else if (getGamesRequest.PageListModel.SortBy == SortValue.DATECREATED)
                        {
                            if (!getGamesRequest.PageListModel.OrderByDescending)
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderBy(a => a.DateCreated)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Games = (List<IGame>)response.Objects
                                    .ConvertAll(g => (IGame)g)
                                    .OrderByDescending(a => a.DateCreated)
                                    .Skip((getGamesRequest.PageListModel.Page - 1) * getGamesRequest.PageListModel.ItemsPerPage)
                                    .Take(getGamesRequest.PageListModel.ItemsPerPage);
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = sortValueNotImplementedMessage;

                            return result;
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
            var result = new GameResult();

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
                            result.Game = (Game)gameResponse.Object;

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
                        int index = 1;

                        foreach (var cell in ((Game)gameResponse.Object).SudokuMatrix.SudokuCells)
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

                            index++;
                        }

                        ((Game)gameResponse.Object).IsSolved();

                        var updateGameResponse = await gamesRepository.Update(((Game)gameResponse.Object));

                        if (updateGameResponse.Success)
                        {
                            result.Success = updateGameResponse.Success;
                            result.Game = (Game)updateGameResponse.Object;

                            return result;
                        }
                        else if(!updateGameResponse.Success && updateGameResponse.Exception != null)
                        {
                            result.Success = updateGameResponse.Success;
                            result.Message = updateGameResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = unableToUpdateGameMessage;

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
