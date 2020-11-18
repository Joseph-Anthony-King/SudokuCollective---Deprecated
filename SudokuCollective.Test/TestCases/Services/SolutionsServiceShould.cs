using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class SolutionsServiceShould
    {
        private DatabaseContext context;
        private MockSolutionsRepository MockSolutionsRepository;
        private MockUsersRepository MockUsersRepository;
        private ISolutionsService sut;
        private ISolutionsService sutFailure;
        private string license;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            MockSolutionsRepository = new MockSolutionsRepository(context);
            MockUsersRepository = new MockUsersRepository(context);

            sut = new SolutionsService(
                MockSolutionsRepository.SolutionsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object);

            sutFailure = new SolutionsService(
                MockSolutionsRepository.SolutionsRepositoryFailedRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object);

            license = TestObjects.GetLicense();
            baseRequest = TestObjects.GetBaseRequest();
        }

        [Test]
        [Category("Services")]
        public async Task GetASolution()
        {
            // Arrange

            // Act
            var result = await sut.GetSolution(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Solution Found"));
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfGetSolutionFails()
        {
            // Arrange

            // Act
            var result = await sutFailure.GetSolution(1);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Solution Not Found"));
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
        }

        [Test]
        [Category("Services")]
        public async Task GetSolutions()
        {
            // Arrange

            // Act
            var result = await sut.GetSolutions(baseRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Solutions Found"));
            Assert.That(result.Solutions, Is.TypeOf<List<ISudokuSolution>>());
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfGetSolutionsFails()
        {
            // Arrange

            // Act
            var result = await sutFailure.GetSolutions(baseRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Solutions Not Found"));
            Assert.That(result.Solutions, Is.TypeOf<List<ISudokuSolution>>());
        }

        [Test]
        [Category("Services")]
        public async Task SolveSudokuMatrices()
        {
            // Arrange
            var solveRequest = new SolveRequest()
            {
                UserId = 1,
                Minutes = 3,
                FirstRow = new int[9] { 0, 2, 0, 5, 0, 0, 8, 7, 6 },
                SecondRow = new int[9] { 7, 0, 0, 1, 8, 0, 0, 5, 0 },
                ThirdRow = new int[9] { 8, 5, 9, 7, 0, 0, 0, 4, 0 },
                FourthRow = new int[9] { 5, 9, 0, 0, 0, 4, 6, 8, 1 },
                FifthRow = new int[9] { 0, 1, 0, 0, 3, 0, 0, 0, 0 },
                SixthRow = new int[9] { 0, 0, 0, 8, 6, 0, 0, 9, 5 },
                SeventhRow = new int[9] { 2, 0, 7, 0, 0, 8, 0, 0, 9 },
                EighthRow = new int[9] { 9, 0, 4, 0, 0, 7, 2, 0, 8 },
                NinthRow = new int[9] { 0, 0, 0, 0, 0, 2, 4, 6, 0 },
                License = license,
                RequestorId = 1
            };

            // Act
            var result = await sut.Solve(solveRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Solution Solved"));
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
        }

        [Test]
        [Category("Services")]
        public async Task GenerateASolution()
        {
            // Arrange

            // Act
            var result = await sut.Generate();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Solution Generated"));
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
        }

        [Test]
        [Category("Services")]
        public async Task AddSolutions()
        {
            // Arrange

            // Act
            var result = await sut.AddSolutions(10);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Solutions Added"));
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfAddSolutionsFails()
        {
            // Arrange

            // Act
            var result = await sutFailure.AddSolutions(10);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Object reference not set to an instance of an object."));
        }
    }
}
