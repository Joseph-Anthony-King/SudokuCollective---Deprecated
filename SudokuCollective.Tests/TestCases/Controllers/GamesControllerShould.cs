using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Tests.MockServices;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class GamesControllerShould {

        private DatabaseContext context;
        private GamesController sutSuccess;
        private GamesController sutFailure;
        private MockGamesService mockGamesService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private CreateGameRequest createGameRequest;
        private UpdateGameRequest updateGameRequest;
        private GetMyGameRequest getMyGameRequest;

        [SetUp]
        public async Task Setup() {

            context = await TestDatabase.GetDatabaseContext();
            mockGamesService = new MockGamesService(context);
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            createGameRequest = new CreateGameRequest();

            getMyGameRequest = new GetMyGameRequest() {

                UserId = 1,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
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
        public void SuccessfullyGetGame() {

            // Arrange
            var gameId = 1;

            // Act
            var result = sutSuccess.GetGame(gameId, baseRequest);
            var game = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGameFail() {

            // Arrange
            var gameId = 1;

            // Act
            var result = sutFailure.GetGame(gameId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving game"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGames() {

            // Arrange

            // Act
            var result = sutSuccess.GetGames(baseRequest, true);
            var games = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(((List<Game>)games).Count, Is.EqualTo(2));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGamesFail() {

            // Arrange

            // Act
            var result = sutFailure.GetGames(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving games"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteGames () {

            // Arrange

            // Act
            var result = sutSuccess.DeleteGame(1, baseRequest);
            var statusCode = ((OkResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteGamesFail () {

            // Arrange

            // Act
            var result = sutFailure.DeleteGame(1, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deleting game"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateGames() {

            // Arrange

            // Act
            var result = sutSuccess.PutGame(1, updateGameRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateGamesFail() {

            // Arrange

            // Act
            var result = sutFailure.PutGame(1, updateGameRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating game"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateGames() {

            // Arrange

            // Act
            var result = sutSuccess.PostGame(createGameRequest, true);
            var game = ((CreatedAtActionResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateGamesFail() {

            // Arrange

            // Act
            var result = sutFailure.PostGame(createGameRequest, true);
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCheckGames() {

            // Arrange

            // Act
            var result = sutSuccess.CheckGame(1, updateGameRequest);
            var game = ((OkObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCheckGamesFail() {

            // Arrange

            // Act
            var result = sutFailure.CheckGame(1, updateGameRequest);
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGameByUserId() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.GetMyGame(userId, getMyGameRequest, true);
            var game = ((OkObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(game, Is.InstanceOf<Game>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetGameByUserIdFail() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.GetMyGame(userId, getMyGameRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving game"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetGamesByUserId() {

            // Arrange

            // Act
            var result = sutSuccess.GetMyGames(getMyGameRequest, true);
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
        public void IssueErrorAndMessageShouldGetGamesByUserIdFail() {

            // Arrange

            // Act
            var result = sutFailure.GetMyGames(getMyGameRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Game>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving games"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteGameByUserId() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.DeleteMyGame(userId, getMyGameRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteGameByUserIdFail() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.DeleteMyGame(userId, getMyGameRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Game>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deleting game"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
