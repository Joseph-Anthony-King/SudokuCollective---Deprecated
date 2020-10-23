using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Domain.Models;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class DifficultiesServiceShould
    {
        private DatabaseContext _context;
        private IDifficultiesService sut;
        private string license;

        [SetUp]
        public async Task Setup()
        {
            _context = await TestDatabase.GetDatabaseContext();
            sut = new DifficultiesService(_context);
            license = TestObjects.GetLicense();
        }

        [Test]
        [Category("Services")]
        public async Task GetADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.GetDifficulty(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Difficulty, Is.TypeOf<Difficulty>());
        }

        [Test]
        [Category("Services")]
        public async Task GetDifficulties()
        {
            // Arrange

            // Act
            var result = await sut.GetDifficulties();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Difficulties.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Services")]
        public async Task GetDifficultiesWithoutNullOrTestDifficultyLevel()
        {
            // Arrange

            // Act
            var result = await sut.GetDifficulties();
            var nullAndTestDifficultyLevelsBlocked = result.Difficulties
                .Any(difficulty =>
                    difficulty.DifficultyLevel.Equals(DifficultyLevel.NULL)
                    || difficulty.DifficultyLevel.Equals(DifficultyLevel.TEST));

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Difficulties.Count, Is.EqualTo(4));
            Assert.That(nullAndTestDifficultyLevelsBlocked, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task CreateADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.CreateDifficulty(
                "testDifficulty", DifficultyLevel.NULL);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Difficulty, Is.TypeOf<Difficulty>());
        }

        [Test]
        [Category("Services")]
        public async Task UpdateADifficulty()
        {
            // Arrange
            var updateDifficultyRequest = new UpdateDifficultyRequest()
            {
                Id = 1,
                Name = "Null UPDATED!",
                DifficultyLevel = DifficultyLevel.NULL,
                License = license,
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            // Act
            var result = await sut.UpdateDifficulty(1, updateDifficultyRequest);
            var updatedDifficulty = _context.Difficulties
                .FirstOrDefault(predicate: difficulty => difficulty.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(updatedDifficulty.Name, Is.EqualTo("Null UPDATED!"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.DeleteDifficulty(1);
            var difficulties = _context.Difficulties.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(difficulties.Count, Is.EqualTo(5));
        }
    }
}
