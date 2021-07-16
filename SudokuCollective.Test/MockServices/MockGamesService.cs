using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Data.Messages;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Enums;
using System.Collections.Generic;

namespace SudokuCollective.Test.MockServices
{
    public class MockGamesService
    {
        private MockGamesRepository MockGamesRepository { get; set; }

        internal Mock<IGamesService> GamesServiceSuccessfulRequest { get; set; }
        internal Mock<IGamesService> GamesServiceFailedRequest { get; set; }
        internal Mock<IGamesService> GamesServiceUpdateFailedRequest { get; set; }

        public MockGamesService(DatabaseContext context)
        {
            MockGamesRepository = new MockGamesRepository(context);

            GamesServiceSuccessfulRequest = new Mock<IGamesService>();
            GamesServiceFailedRequest = new Mock<IGamesService>();
            GamesServiceUpdateFailedRequest = new Mock<IGamesService>();

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.Create(It.IsAny<CreateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameCreatedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Game>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.Update(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameUpdatedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameDeletedMessage
                } as IBaseResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameFoundMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object,
                } as IGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetGames(It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GamesResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = GamesMessages.GamesFoundMessage,
                    Games = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(g => (IGame)g)
                } as IGamesResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetMyGame(It.IsAny<int>(), It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameFoundMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetMyGames(It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GamesResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetMyGames(It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GamesFoundMessage,
                    Games = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .GetMyGames(It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Objects
                        .ConvertAll(g => (IGame)g)
                } as IGamesResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.DeleteMyGame(It.IsAny<int>(), It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .DeleteMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameDeletedMessage
                } as IBaseResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.Check(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameSolvedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.CreateAnnonymous(It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new AnnonymousGameResult()
                {
                    Success = true,
                    Message = GamesMessages.GameCreatedMessage,
                    SudokuMatrix = new List<List<int>>(),
                } as IAnnonymousGameResult));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.CheckAnnonymous(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = GamesMessages.GameSolvedMessage
                } as IBaseResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.Create(It.IsAny<CreateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotCreatedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Game>())
                        .Result
                        .Object,
                } as IGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.Update(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotUpdatedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotDeletedMessage
                } as IBaseResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetAppGame(It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotFoundMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetAppGame(It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Object,
                } as IGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetGames(It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GamesResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetAppGames(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GamesNotFoundMessage,
                    Games = null
                } as IGamesResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetMyGame(It.IsAny<int>(), It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotFoundMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetMyGames(It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new GamesResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GamesNotFoundMessage,
                    Games = null
                } as IGamesResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.DeleteMyGame(It.IsAny<int>(), It.IsAny<GamesRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .DeleteMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotDeletedMessage
                } as IBaseResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.Check(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {
                    Success = MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Success,
                    Message = GamesMessages.GameNotUpdatedMessage,
                    Game = (Game)MockGamesRepository
                        .GamesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Game>())
                        .Result
                        .Object
                } as IGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.CreateAnnonymous(It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new AnnonymousGameResult()
                {
                    Success = false,
                    Message = GamesMessages.GameNotCreatedMessage,
                    SudokuMatrix = null,
                } as IAnnonymousGameResult));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.CheckAnnonymous(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = GamesMessages.GameNotSolvedMessage
                } as IBaseResult));
        }
    }
}
