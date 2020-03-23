using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Services;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Services {

    public class GamesServiceShould {

        private DatabaseContext _context;
        private IGamesService sut;
        private DateTime dateCreated;
        private string license;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();

            sut = new GamesService(_context);

            dateCreated = DateTime.UtcNow;
            license = TestObjects.GetLicense();
            baseRequest = TestObjects.GetBaseRequest();
        }

        [Test]
        [Category("Services")]
        public async Task CreateGames() {

            // Arrange
            var createGameRequest = new CreateGameRequest() {

                UserId = 1,
                DifficultyId = 4,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateGame(createGameRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(result.Game.IsSolved(), Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task FailToCreateGameIfUserDoesNotExist() {

            // Arrange
            var createGameRequest = new CreateGameRequest() {

                UserId = 5,
                DifficultyId = 4,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateGame(createGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(result.Game.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateGames() {

            var gameId = 1;
            var updatedValue = 6;

            // Arrange
            var updateGameRequest = new UpdateGameRequest() {
                
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
                .Select(cell => cell.DisplayValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(checkValue, Is.EqualTo(updatedValue));
        }

        [Test]
        [Category("Services")]
        public async Task RejectUpdateIfCellsAreInvalid() {

            // Arrange
            var gameId = 1;
            var updatedValue = 6;

            var updateGameRequest = new UpdateGameRequest() {

                GameId = gameId,
                SudokuCells = TestObjects.GetUpdateInvalidSudokuCells(updatedValue),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateGame(gameId, updateGameRequest);

            var updatedGame = await _context.Games
                .FirstOrDefaultAsync(predicate: game => game.Id == gameId);

            var checkValue = updatedGame.SudokuMatrix.SudokuCells
                .Where(cell => cell.Id == 58)
                .Select(cell => cell.DisplayValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(checkValue, Is.Not.EqualTo(updatedValue));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteGames() {

            // Arrange
            var gameId = 1;

            // Act
            var result = await sut.DeleteGame(gameId);

            var games = _context.Games.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(games.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteReturnsErrorMessageIfGameNotFound() {

            // Arrange
            var gameId = 5;

            // Act
            var result = await sut.DeleteGame(gameId);

            var games = _context.Games.ToList();

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not found"));
            Assert.That(games.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task GetAGame() {

            // Arrange
            var gameId = 1;
            var appId = 1;

            // Act
            var result = await sut.GetGame(gameId, appId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task ReturnErrorMessageIfGameNotFound() {

            // Arrange
            var gameId = 5;
            var appId = 1;

            // Act
            var result = await sut.GetGame(gameId, appId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not found"));
            Assert.That(result.Game.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task GetGames() {

            // Arrange

            // Act
            var result = await sut.GetGames(baseRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Games.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersGame() {

            // Arrange
            var userId = 1;
            var gameId = 1;
            var appId = 1;

            // Act
            var result = await sut.GetMyGame(userId, gameId, appId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersGames() {

            // Arrange
            var userId = 1;
            var getMyGameRequest = new GetMyGameRequest() {

                UserId = userId
            };

            // Act
            var result = await sut.GetMyGames(userId, getMyGameRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Games.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteAUsersGame() {

            // Arrange
            var userId = 1;
            var gameId = 1;

            // Act
            var result = await sut.DeleteMyGame(userId, gameId);
            var usersGame = _context.Games
                .Where(game => game.UserId == userId)
                .ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(usersGame.Count, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task CheckGames() {

            var gameId = 1;
            var updatedValue = 6;

            // Arrange
            var updateGameRequest = new UpdateGameRequest() {

                GameId = gameId,
                SudokuCells = TestObjects.GetUpdateSudokuCells(updatedValue),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CheckGame(gameId, updateGameRequest);

            var updatedGame = await _context.Games
                .FirstOrDefaultAsync(predicate: game => game.Id == gameId);

            var checkValue = updatedGame.SudokuMatrix.SudokuCells
                .Where(cell => cell.Id == 58)
                .Select(cell => cell.DisplayValue)
                .FirstOrDefault();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(checkValue, Is.EqualTo(updatedValue));
        }

        [Test]
        [Category("Services")]
        public async Task NoteWhenGameIsSolvedOnUpdate() {

            var gameId = 1;

            // Arrange
            var updateGameRequest = new UpdateGameRequest() {

                GameId = gameId,
                SudokuCells = TestObjects.GetSolvedSudokuCells(),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CheckGame(gameId, updateGameRequest);

            var updatedGame = await _context.Games
                .FirstOrDefaultAsync(predicate: game => game.Id == gameId);

            var game = _context.Games.FirstOrDefault(predicate: g => g.Id == gameId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(game.IsSolved(), Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task CheckGameShouldReturnMessageIfGameNotFound() {

            var gameId = 5;

            // Arrange
            var updateGameRequest = new UpdateGameRequest() {

                GameId = gameId,
                SudokuCells = TestObjects.GetSolvedSudokuCells(),
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CheckGame(gameId, updateGameRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Game not found"));
            Assert.That(result.Game, Is.TypeOf<Game>());
            Assert.That(result.Game.Id, Is.EqualTo(0));
        }
    }
}
