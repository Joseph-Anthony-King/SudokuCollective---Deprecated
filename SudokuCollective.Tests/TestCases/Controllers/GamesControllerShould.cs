using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class GamesControllerShould {

        private DatabaseContext context;
        private GamesController sut;
        private Mock<IGamesService> mockGamesService;
        private Mock<IAppsService> mockAppsService;
        private BaseRequest baseRequest;
        private CreateGameRequest createGameRequest;
        private UpdateGameRequest updateGameRequest;
        private GetMyGameRequest getMyGameRequest;

        [SetUp]
        public async Task Setup() {

            context = await TestDatabase.GetDatabaseContext();
            baseRequest = new BaseRequest();
            createGameRequest = new CreateGameRequest();
            getMyGameRequest = new GetMyGameRequest() {

                UserId = 1,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };
            updateGameRequest = TestObjects.GetUpdateGameRequest();

            mockGamesService = new Mock<IGamesService>();

            mockGamesService.Setup(gameService =>
                gameService.GetGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game()
                }));

            mockGamesService.Setup(gameService =>
                gameService.GetGames(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Games = context.Games.ToList()
                }));

            mockGamesService.Setup(gameService =>
                gameService.DeleteGame(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockGamesService.Setup(gameService =>
                gameService.UpdateGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game()
                }));

            mockGamesService.Setup(gameService =>
                gameService.CreateGame(It.IsAny<CreateGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult() {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game()
                }));

            mockGamesService.Setup(gameService =>
                gameService.CheckGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .Returns(Task.FromResult(new GameResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game()
                }));

            mockGamesService.Setup(gameService =>
                gameService.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GameResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Game = new Game()
                }));

            mockGamesService.Setup(gameService =>
                gameService.GetMyGames(It.IsAny<int>(), It.IsAny<GetMyGameRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GamesResult() {

                    Success = true,
                    Message = string.Empty,
                    Games = context.Games.Where(g => g.UserId == 1).ToList()
                }));

            mockGamesService.Setup(gameService =>
                gameService.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockAppsService = new Mock<IAppsService>();

            mockAppsService.Setup(appService =>
                appService.IsRequestValidOnThisLicense(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            sut = new GamesController(mockGamesService.Object, mockAppsService.Object);
        }

        [Test]
        [Category("Controllers")]
        public void GetAGame() {

            // Arrange
            var gameId = 1;

            // Act
            var result = sut.GetGame(gameId, baseRequest);
            var processResult = result.Result;
            var game = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void GetGames() {

            // Arrange

            // Act
            var result = sut.GetGames(baseRequest, true);
            var processResult = result.Result;
            var games = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(((List<Game>)games).Count, Is.EqualTo(2));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void DeleteGames () {

            // Arrange

            // Act
            var result = sut.DeleteGame(1, baseRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void UpdateGames() {

            // Arrange

            // Act
            var result = sut.PutGame(1, updateGameRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void CreateGames() {

            // Arrange

            // Act
            var result = sut.PostGame(createGameRequest, true);
            var game = ((CreatedAtActionResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void CheckGames() {

            // Arrange

            // Act
            var result = sut.CheckGame(1, updateGameRequest);
            var game = ((OkObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void GetGameByUserId() {

            // Arrange
            var userId = 1;

            // Act
            var result = sut.GetMyGame(userId, getMyGameRequest, true);
            var game = ((OkObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void GetGamesByUserId() {

            // Arrange

            // Act
            var result = sut.GetMyGames(getMyGameRequest, true);
            var processResult = result.Result;
            var games = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(((List<Game>)games).Count, Is.EqualTo(1));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void DeleteGameByUserId() {

            // Arrange
            var userId = 1;

            // Act
            var result = sut.DeleteMyGame(userId, getMyGameRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
