using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class SolutionsServiceShould
    {
        private DatabaseContext _context;
        private ISolutionsService sut;
        private string license;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup()
        {
            _context = await TestDatabase.GetDatabaseContext();
            sut = new SolutionsService(_context);
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
            Assert.That(result.Solutions.Count, Is.EqualTo(0));
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
            var solutionsResult = _context.SudokuSolutions.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
            Assert.That(solutionsResult.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Services")]
        public async Task GenerateASolution()
        {
            // Arrange

            // Act
            var result = await sut.Generate();
            var solutionsResult = _context.SudokuSolutions.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Solution, Is.TypeOf<SudokuSolution>());
            Assert.That(solutionsResult.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Services")]
        public async Task AddSolutions()
        {
            // Arrange
            var limit = 10;

            // Act
            var result = await sut.AddSolutions(limit);
            var solutionsResult = _context.SudokuSolutions.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(solutionsResult.Count, Is.EqualTo(limit + 3));
        }
    }
}
