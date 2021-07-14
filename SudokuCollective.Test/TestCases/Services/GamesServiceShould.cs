using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class GamesServiceShould
    {
        private DatabaseContext context;
        private MockGamesRepository MockGamesRepository;
        private MockAppsRepository MockAppsRepository;
        private MockUsersRepository MockUsersRepository;
        private MockDifficultiesRepository MockDifficultiesRepositorySuccessful;
        private MockDifficultiesRepository MockDifficultiesRepositoryFailed;
        private IGamesService sut;
        private IGamesService sutFailure;
        private IGamesService sutAnonFailure;
        private IGamesService sutUpdateFailure;
        private GamesRequest getGamesRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            MockGamesRepository = new MockGamesRepository(context);
            MockAppsRepository = new MockAppsRepository(context);
            MockUsersRepository = new MockUsersRepository(context);
            MockDifficultiesRepositorySuccessful = new MockDifficultiesRepository(context);
            MockDifficultiesRepositoryFailed = new MockDifficultiesRepository(context);

            sut = new GamesService(
                MockGamesRepository.GamesRepositorySuccessfulRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                MockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object);

            sutFailure = new GamesService(
                MockGamesRepository.GamesRepositoryFailedRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                MockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object);

            sutAnonFailure = new GamesService(
                MockGamesRepository.GamesRepositorySuccessfulRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                MockDifficultiesRepositoryFailed.DifficultiesRepositoryFailedRequest.Object);

            sutUpdateFailure = new GamesService(
                MockGamesRepository.GamesRepositoryUpdateFailedRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                MockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object);

            getGamesRequest = TestObjects.GetGamesRequest();
        }

        [Test]
        [Category("Services")]
        public async Task CreateGames()
        {
            // Arrange
            var createGameRequest = new CreateGameRequest()
            {
                UserId = 1,
                DifficultyId = 4,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateGame(createGameRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Created"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task FailToCreateGameIfUserDoesNotExist()
        {
            // Arrange
            var createGameRequest = new CreateGameRequest()
            {
                UserId = 5,
                DifficultyId = 4,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sutFailure.CreateGame(createGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game Not Created"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task UpdateGames()
        {
            var gameId = 1;
            var updatedValue = 6;

            // Arrange
            var updateGameRequest = new UpdateGameRequest()
            {
                GameId = gameId,
                SudokuCells = TestObjects.GetUpdateSudokuCells(updatedValue),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateGame(gameId, updateGameRequest);

            var checkValue = result.Game.SudokuMatrix.SudokuCells
                .OrderBy(cell => cell.Index)
                .Where(cell => cell.Index == 2)
                .Select(cell => cell.DisplayedValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Updated"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(checkValue, Is.EqualTo(updatedValue));
        }

        [Test]
        [Category("Services")]
        public async Task RejectUpdateIfCellsAreInvalid()
        {
            // Arrange
            var gameId = 1;
            var updatedValue = 6;

            var updateGameRequest = new UpdateGameRequest()
            {
                GameId = gameId,
                SudokuCells = TestObjects.GetUpdateInvalidSudokuCells(updatedValue),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sutUpdateFailure.UpdateGame(gameId, updateGameRequest);

            var updatedGame = await context.Games
                .FirstOrDefaultAsync(game => game.Id == gameId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game Not Updated"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task DeleteGames()
        {
            // Arrange
            var gameId = 1;

            // Act
            var result = await sut.DeleteGame(gameId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Deleted"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteReturnsErrorMessageIfGameNotFound()
        {
            // Arrange
            var gameId = 5;

            // Act
            var result = await sutFailure.DeleteGame(gameId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game Not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task GetAGame()
        {
            // Arrange
            var gameId = 1;
            var appId = 1;

            // Act
            var result = await sut.GetGame(gameId, appId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Found"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task ReturnErrorMessageIfGameNotFound()
        {
            // Arrange
            var gameId = 5;
            var appId = 1;

            // Act
            var result = await sutFailure.GetGame(gameId, appId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game Not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task GetGames()
        {
            // Arrange

            // Act
            var result = await sut.GetGames(getGamesRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Games Found"));
            Assert.That(result.Games, Is.TypeOf<List<IGame>>());
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersGame()
        {
            // Arrange
            var gameId = 1;

            // Act
            var result = await sut.GetMyGame(gameId, getGamesRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Found"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersGames()
        {
            // Arrange
            var userId = 1;
            var getMyGameRequest = new GamesRequest()
            {
                Paginator = new Paginator(),
                UserId = userId
            };

            // Act
            var result = await sut.GetMyGames(getMyGameRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Games Found"));
            Assert.That(result.Games, Is.TypeOf<List<IGame>>());
        }

        [Test]
        [Category("Services")]
        public async Task DeleteAUsersGame()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;

            // Act
            var result = await sut.DeleteMyGame(gameId, getGamesRequest);
            var usersGame = context.Games
                .Where(game => game.UserId == userId)
                .ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Deleted"));
        }

        [Test]
        [Category("Services")]
        public async Task CheckGames()
        {
            var gameId = 1;
            var updatedValue = 6;

            // Arrange
            var updateGameRequest = new UpdateGameRequest()
            {

                GameId = gameId,
                SudokuCells = TestObjects.GetUpdateSudokuCells(updatedValue),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CheckGame(gameId, updateGameRequest);

            var updatedGame = await context.Games
                .FirstOrDefaultAsync(game => game.Id == gameId);

            var checkValue = updatedGame.SudokuMatrix.SudokuCells
                .Where(cell => cell.Id == 58)
                .Select(cell => cell.DisplayedValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Not Solved"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(checkValue, Is.EqualTo(updatedValue));
        }

        [Test]
        [Category("Services")]
        public async Task NoteWhenGameIsSolvedOnUpdate()
        {
            var gameId = 1;

            // Arrange
            var updateGameRequest = new UpdateGameRequest()
            {
                GameId = gameId,
                SudokuCells = TestObjects.GetSolvedSudokuCells(),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CheckGame(gameId, updateGameRequest);

            var updatedGame = await context.Games
                .FirstOrDefaultAsync(game => game.Id == gameId);

            var game = context.Games.FirstOrDefault(g => g.Id == gameId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Solved"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(game.IsSolved(), Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task CheckGameShouldReturnMessageIfGameNotFound()
        {
            var gameId = 5;

            // Arrange
            var updateGameRequest = new UpdateGameRequest()
            {
                GameId = gameId,
                SudokuCells = TestObjects.GetSolvedSudokuCells(),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sutFailure.CheckGame(gameId, updateGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game Not Found"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(result.Game.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task CreateAnnonymousGames()
        {
            // Arrange

            // Act
            var result = await sut.CreateAnnonymousGame(DifficultyLevel.TEST);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Created"));
            Assert.That(result.SudokuMatrix, Is.TypeOf<List<List<int>>>());
        }

        [Test]
        [Category("Services")]
        public async Task CreateAnnonymousGamesShouldReturnMessageIfDifficultyNotFound()
        {
            // Arrange
            var difficulty = await context
                .Difficulties
                .FirstOrDefaultAsync(d => d.DifficultyLevel == DifficultyLevel.TEST);

            context.Difficulties.Remove(difficulty);

            // Act
            var result = await sutAnonFailure.CreateAnnonymousGame(difficulty.DifficultyLevel);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Difficulty Not Found"));
        }
    }
}
