using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.TestData;
using SudokuCollective.Test.MockRepositories;

namespace SudokuCollective.Test.TestCases.Services
{
    public class DifficultiesServiceShould
    {
        private DatabaseContext context;
        private MockDifficultiesRepository MockDifficultiesRepository;
        private MemoryDistributedCache memoryCache;
        private IDifficultiesService sut;
        private IDifficultiesService sutCreateDifficulty;
        private string license;
        private Paginator paginator;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            MockDifficultiesRepository = new MockDifficultiesRepository(context);
            memoryCache = new MemoryDistributedCache(
                Options.Create(new MemoryDistributedCacheOptions()));

            sut = new DifficultiesService(
                MockDifficultiesRepository.DifficultiesRepositorySuccessfulRequest.Object,
                memoryCache);
            sutCreateDifficulty = new DifficultiesService(
                MockDifficultiesRepository.DifficultiesRepositoryCreateDifficultyRequest.Object,
                memoryCache);
            license = TestObjects.GetLicense();
            paginator = TestObjects.GetPaginator();
        }

        [Test]
        [Category("Services")]
        public async Task GetADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.Get(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Difficulty Found"));
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
            Assert.That(result.Message, Is.EqualTo("Difficulties Found"));
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
            Assert.That(result.Message, Is.EqualTo("Difficulties Found"));
            Assert.That(nullAndTestDifficultyLevelsBlocked, Is.False);
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
                License = license,
                RequestorId = 1,
                Paginator = new Paginator()
            };

            // Act
            var result = await sut.Update(1, updateDifficultyRequest);
            var updatedDifficulty = context.Difficulties
                .FirstOrDefault(difficulty => difficulty.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Difficulty Updated"));
            Assert.That(updatedDifficulty.Name, Is.EqualTo("Null"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.Delete(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Difficulty Deleted"));
        }

        [Test]
        [Category("Services")]
        public async Task CreateADifficulty()
        {
            // Arrange

            // Act
            var result = await sutCreateDifficulty
                .Create("Test", "Test", DifficultyLevel.TEST);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Difficulty Created"));
            Assert.That(result.Difficulty, Is.InstanceOf<Difficulty>());
        }
    }
}
