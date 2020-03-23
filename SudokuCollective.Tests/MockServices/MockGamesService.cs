using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockGamesService {

        private DatabaseContext _context;
        internal Mock<IGamesService> GamesServiceSuccessfulRequest { get; set; }
        internal Mock<IGamesService> GamesServiceFailedRequest { get; set; }

        public MockGamesService(DatabaseContext context) {

            _context = context;
            GamesServiceSuccessfulRequest = new Mock<IGamesService>();
            GamesServiceFailedRequest = new Mock<IGamesService>();

            GamesServiceSuccessfulRequest.Setup(gamesService => 
                gamesService.CreateGame(It.IsAny<CreateGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game(3, 1, 3, 3, 1, true, DateTime.UtcNow, DateTime.MinValue)
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.UpdateGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = _context.Games.FirstOrDefault(predicate: game => game.Id == 1)
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService => 
                gamesService.DeleteGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = _context.Games.FirstOrDefault(predicate: game => game.Id == 1)
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetGames(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult() {

                    Success = true,
                    Message = string.Empty,
                    Games = _context.Games.ToList()
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService =>
                gamesService.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = _context.Games.FirstOrDefault(game => game.Id == 1 && game.UserId == 1)
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService => 
                gamesService.GetMyGames(It.IsAny<int>(), It.IsAny<GetMyGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult() {

                    Success = true,
                    Message = string.Empty,
                    Games = _context.Games.Where(game => game.UserId == 1).ToList()
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService => 
                gamesService.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            GamesServiceSuccessfulRequest.Setup(gamesService => 
                gamesService.CheckGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = _context.Games.FirstOrDefault(predicate: game => game.Id == 1)
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.CreateGame(It.IsAny<CreateGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = false,
                    Message = "Error creating game",
                    Game = new Game()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.UpdateGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = false,
                    Message = "Error updating game",
                    Game = new Game()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.DeleteGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting game"
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = false,
                    Message = "Error retrieving game",
                    Game = new Game()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetGames(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult() {

                    Success = false,
                    Message = "Error retrieving games",
                    Games = new List<Game>()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = false,
                    Message = "Error retrieving game",
                    Game = new Game()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.GetMyGames(It.IsAny<int>(), It.IsAny<GetMyGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult() {

                    Success = false,
                    Message = "Error retrieving games",
                    Games = new List<Game>()
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting game"
                }));

            GamesServiceFailedRequest.Setup(gamesService =>
                gamesService.CheckGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = false,
                    Message = "Error checking game",
                    Game = new Game()
                }));
        }
    }
}
