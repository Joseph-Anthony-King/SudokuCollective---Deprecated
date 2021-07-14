using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Models.RequestModels.GameRequests;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class GamesControllerShould
    {
        private DatabaseContext context;
        private GamesController sutSuccess;
        private GamesController sutFailure;
        private MockGamesService mockGamesService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private CreateGameRequest createGameRequest;
        private UpdateGameRequest updateGameRequest;
        private GamesRequest getMyGameRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockGamesService = new MockGamesService(context);
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            createGameRequest = new CreateGameRequest();

            getMyGameRequest = new GamesRequest()
            {
                UserId = 1,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
            };

            updateGameRequest = TestObjects.GetUpdateGameRequest();

            sutSuccess = new GamesController(
                mockGamesService.GamesServiceSuccessfulRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutFailure = new GamesController(
                mockGamesService.GamesServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGame()
        {
            // Arrange
            var gameId = 1;

            // Act
            var result = sutSuccess.GetGame(gameId, baseRequest);
            var message = ((GameResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var game = ((GameResult)((OkObjectResult)result.Result.Result).Value).Game;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGameFail()
        {
            // Arrange
            var gameId = 1;

            // Act
            var result = sutFailure.GetGame(gameId, baseRequest);
            var message = ((GameResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGames()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetGames(getMyGameRequest, true);
            var message = ((GamesResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Games Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetGames(getMyGameRequest, true);
            var message = ((GamesResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Games not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteGames()
        {
            // Arrange

            // Act
            var result = sutSuccess.DeleteGame(1, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.DeleteGame(1, baseRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Deleted"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateGames()
        {
            // Arrange

            // Act
            var result = sutSuccess.UpdateGame(1, updateGameRequest);
            var message = ((GameResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Updated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.UpdateGame(1, updateGameRequest);
            var message = ((GameResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateGames()
        {
            // Arrange

            // Act
            var result = sutSuccess.PostGame(createGameRequest);
            var message = ((GameResult)((ObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result.Result).StatusCode;
            var game = ((GameResult)((ObjectResult)result.Result.Result).Value).Game;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 201: Game Created"));
            Assert.That(statusCode, Is.EqualTo(201));
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PostGame(createGameRequest);
            var message = ((GameResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Created"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCheckGames()
        {
            // Arrange

            // Act
            var result = sutSuccess.CheckGame(1, updateGameRequest);
            var message = ((GameResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var game = ((GameResult)((OkObjectResult)result.Result.Result).Value).Game;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Solved"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCheckGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.CheckGame(1, updateGameRequest);
            var message = ((GameResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGameByUserId()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.GetMyGame(userId, getMyGameRequest, true);
            var message = ((GameResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var game = ((GameResult)((OkObjectResult)result.Result.Result).Value).Game;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGameByUserIdFail()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.GetMyGame(userId, getMyGameRequest, true);
            var message = ((GameResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGamesByUserId()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetMyGames(getMyGameRequest, true);
            var message = ((GamesResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Games Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGamesByUserIdFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetMyGames(getMyGameRequest, true);
            var message = ((GamesResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Games not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteGameByUserId()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.DeleteMyGame(userId, getMyGameRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteGameByUserIdFail()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.DeleteMyGame(userId, getMyGameRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Deleted"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateAnnonymousGames()
        {
            // Arrange

            // Assert
            var result = sutSuccess.PostAnnonymousGame(
                new AnnonymousGameRequest { 
                    DifficultyLevel = DifficultyLevel.TEST 
                });
            var message = ((AnnonymousGameResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Game Created"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueMessageShouldCreateAnnonymousGamesFail()
        {
            // Arrange

            // Assert
            var result = sutFailure.PostAnnonymousGame(
                new AnnonymousGameRequest
                {
                    DifficultyLevel = DifficultyLevel.TEST
                });
            var message = ((AnnonymousGameResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Created"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCheckAnnonymousGames()
        {
            // Arrange

            // Assert
            var result = sutSuccess.CheckAnnonymousGame(
                new AnnonymousCheckRequest
                {
                    FirstRow = new List<int> { 2, 9, 8, 1, 3, 4, 6, 7, 5 },
                    SecondRow = new List<int> { 3, 1, 6, 5, 8, 7, 2, 9, 4 },
                    ThirdRow = new List<int> { 4, 5, 7, 6, 9, 2, 1, 8, 3  },
                    FourthRow = new List<int> { 9, 7, 1, 2, 4, 3, 5, 6, 8 },
                    FifthRow = new List<int> { 5, 8, 3, 7, 6, 1, 4, 2, 9 },
                    SixthRow = new List<int> { 6, 2, 4, 9, 5, 8, 3, 1, 7 },
                    SeventhRow = new List<int> { 7, 3, 5, 8, 2, 6, 9, 4, 1 },
                    EighthRow = new List<int> { 8, 4, 2, 3, 1, 9, 7, 5, 6 },
                    NinthRow = new List<int> { 1, 6, 9, 4, 7, 5, 8, 3, 2 }
                });

            var success = ((BaseResult)((ObjectResult)result.Result).Value).Success;
            var message = ((BaseResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Status Code 200: Game Solved"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueMessageShouldCheckAnnonymousGamesFail()
        {
            // Arrange

            // Assert
            var result = sutFailure.CheckAnnonymousGame(
                new AnnonymousCheckRequest
                {
                    FirstRow = new List<int> { 5, 9, 8, 1, 3, 4, 6, 7, 2 },
                    SecondRow = new List<int> { 3, 1, 6, 5, 8, 7, 2, 9, 4 },
                    ThirdRow = new List<int> { 4, 5, 7, 6, 9, 2, 1, 8, 3 },
                    FourthRow = new List<int> { 9, 7, 1, 2, 4, 3, 5, 6, 8 },
                    FifthRow = new List<int> { 5, 8, 3, 7, 6, 1, 4, 2, 9 },
                    SixthRow = new List<int> { 6, 2, 4, 9, 5, 8, 3, 1, 7 },
                    SeventhRow = new List<int> { 7, 3, 5, 8, 2, 6, 9, 4, 1 },
                    EighthRow = new List<int> { 8, 4, 2, 3, 1, 9, 7, 5, 6 },
                    NinthRow = new List<int> { 1, 6, 9, 4, 7, 5, 8, 3, 2 }
                });

            var success = ((BaseResult)((ObjectResult)result.Result).Value).Success;
            var message = ((BaseResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Status Code 404: Game not Solved"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
