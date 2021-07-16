using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
        private MockGamesRepository mockGamesRepository;
        private MockAppsRepository mockAppsRepository;
        private MockUsersRepository mockUsersRepository;
        private MockDifficultiesRepository mockDifficultiesRepositorySuccessful;
        private MockDifficultiesRepository mockDifficultiesRepositoryFailed;
        private MockSolutionsRepository mockSolutionsRepository;
        private MemoryDistributedCache memoryCache;
        private IGamesService sut;
        private IGamesService sutFailure;
        private IGamesService sutAnonFailure;
        private IGamesService sutUpdateFailure;
        private GamesRequest getGamesRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            mockGamesRepository = new MockGamesRepository(context);
            mockAppsRepository = new MockAppsRepository(context);
            mockUsersRepository = new MockUsersRepository(context);
            mockDifficultiesRepositorySuccessful = new MockDifficultiesRepository(context);
            mockDifficultiesRepositoryFailed = new MockDifficultiesRepository(context);
            mockSolutionsRepository = new MockSolutionsRepository(context);
            memoryCache = new MemoryDistributedCache(
                Options.Create(new MemoryDistributedCacheOptions()));

            sut = new GamesService(
                mockGamesRepository.GamesRepositorySuccessfulRequest.Object,
                mockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                mockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                mockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object,
                mockSolutionsRepository.SolutionsRepositorySuccessfulRequest.Object,
                memoryCache);

            sutFailure = new GamesService(
                mockGamesRepository.GamesRepositoryFailedRequest.Object,
                mockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                mockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                mockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object,
                mockSolutionsRepository.SolutionsRepositorySuccessfulRequest.Object,
                memoryCache);

            sutAnonFailure = new GamesService(
                mockGamesRepository.GamesRepositorySuccessfulRequest.Object,
                mockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                mockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                mockDifficultiesRepositoryFailed.DifficultiesRepositoryFailedRequest.Object,
                mockSolutionsRepository.SolutionsRepositorySuccessfulRequest.Object,
                memoryCache);

            sutUpdateFailure = new GamesService(
                mockGamesRepository.GamesRepositoryUpdateFailedRequest.Object,
                mockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                mockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                mockDifficultiesRepositorySuccessful.DifficultiesRepositorySuccessfulRequest.Object,
                mockSolutionsRepository.SolutionsRepositorySuccessfulRequest.Object,
                memoryCache);

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
            var result = await sut.Create(createGameRequest);

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
            var result = await sutFailure.Create(createGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not Created"));
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
            var result = await sut.Update(gameId, updateGameRequest);

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
            var result = await sutUpdateFailure.Update(gameId, updateGameRequest);

            var updatedGame = await context.Games
                .FirstOrDefaultAsync(game => game.Id == gameId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not Updated"));
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task DeleteGames()
        {
            // Arrange
            var gameId = 1;

            // Act
            var result = await sut.Delete(gameId);

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
            var result = await sutFailure.Delete(gameId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not Found"));
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
            Assert.That(result.Message, Is.EqualTo("Game not Found"));
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
            var result = await sut.Check(gameId, updateGameRequest);

            var updatedGame = await context.Games
                .FirstOrDefaultAsync(game => game.Id == gameId);

            var checkValue = updatedGame.SudokuMatrix.SudokuCells
                .Where(cell => cell.Id == 58)
                .Select(cell => cell.DisplayedValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game not Solved"));
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
            var result = await sut.Check(gameId, updateGameRequest);

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
            var result = await sutFailure.Check(gameId, updateGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not Found"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(result.Game.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task CreateAnnonymousGames()
        {
            // Arrange

            // Act
            var result = await sut.CreateAnnonymous(DifficultyLevel.TEST);

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
            var result = await sutAnonFailure.CreateAnnonymous(difficulty.DifficultyLevel);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Difficulty not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task CheckAnnonymousGames()
        {
            // Arrange
            var intList = new List<int> {
                2, 9, 8, 1, 3, 4, 6, 7, 5,
                3, 1, 6, 5, 8, 7, 2, 9, 4,
                4, 5, 7, 6, 9, 2, 1, 8, 3,
                9, 7, 1, 2, 4, 3, 5, 6, 8,
                5, 8, 3, 7, 6, 1, 4, 2, 9,
                6, 2, 4, 9, 5, 8, 3, 1, 7,
                7, 3, 5, 8, 2, 6, 9, 4, 1,
                8, 4, 2, 3, 1, 9, 7, 5, 6,
                1, 6, 9, 4, 7, 5, 8, 3, 2 };

            // Act
            var result = await sut.CheckAnnonymous(intList);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Game Solved"));
        }

        [Test]
        [Category("Services")]
        public async Task CheckAnnonymousGamesShouldReturnMessageIfSolutionNotFound()
        {
            // Arrange
            var intList = new List<int> {
                5, 9, 8, 1, 3, 4, 6, 7, 2,
                3, 1, 6, 5, 8, 7, 2, 9, 4,
                4, 5, 7, 6, 9, 2, 1, 8, 3,
                9, 7, 1, 2, 4, 3, 5, 6, 8,
                5, 8, 3, 7, 6, 1, 4, 2, 9,
                6, 2, 4, 9, 5, 8, 3, 1, 7,
                7, 3, 5, 8, 2, 6, 9, 4, 1,
                8, 4, 2, 3, 1, 9, 7, 5, 6,
                1, 6, 9, 4, 7, 5, 8, 3, 2 };

            // Act
            var result = await sut.CheckAnnonymous(intList);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not Solved"));
        }
    }
}
