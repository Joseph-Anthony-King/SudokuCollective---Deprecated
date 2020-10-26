using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class SolutionsControllerShould
    {
        private DatabaseContext context;
        private SolutionsController sutSuccess;
        private SolutionsController sutFailure;
        private SolutionsController sutSolvedFailure;
        private MockSolutionsService mockSolutionsService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private SolveRequest solveRequest;
        private AddSolutionRequest addSolutionRequest;
        private AddSolutionRequest invalidAddSolutionRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockSolutionsService = new MockSolutionsService(context);
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            solveRequest = new SolveRequest()
            {
                UserId = 1,
                Minutes = 15,
                FirstRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SecondRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                ThirdRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                FourthRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                FifthRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SixthRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SeventhRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                EighthRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                NinthRow = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            addSolutionRequest = new AddSolutionRequest()
            {
                Limit = 1000
            };

            invalidAddSolutionRequest = new AddSolutionRequest()
            {
                Limit = 1001
            };

            sutSuccess = new SolutionsController(
                mockSolutionsService.SolutionsServiceSuccessfulRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutFailure = new SolutionsController(
                mockSolutionsService.SolutionsServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutSolvedFailure = new SolutionsController(
                mockSolutionsService.SolutionsServiceSolveFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetSolution()
        {
            // Arrange
            var solutionId = 1;

            // Act
            var result = sutSuccess.GetSolution(solutionId, baseRequest);
            var solution = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(solution, Is.InstanceOf<SudokuSolution>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetSolutionFail()
        {
            // Arrange
            var solutionId = 2;

            // Act
            var result = sutFailure.GetSolution(solutionId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving solution"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetSolutions()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetSolutions(baseRequest, true);
            var difficulties = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<SudokuSolution>>>());
            Assert.That(((List<ISudokuSolution>)difficulties).Count, Is.EqualTo(3));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetSolutionsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetSolutions(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<SudokuSolution>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving solutions"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullySolveSolution()
        {
            // Arrange

            // Act
            var result = sutSuccess.Solve(solveRequest);
            var solution = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(solution, Is.InstanceOf<SudokuSolution>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSolveSolutionFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Solve(solveRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error solving sudoku matrix"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGenerateSolution()
        {
            // Arrange

            // Act
            var result = sutSuccess.Generate(solveRequest);
            var solution = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(solution, Is.InstanceOf<SudokuSolution>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGenerateSolutionFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Generate(solveRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error generating solution"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void IssueMessageIfSudokuSolutionSolveFailed()
        {
            // Arrange

            // Act
            var result = sutSolvedFailure.Solve(solveRequest);
            var solution = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(solution, Is.EqualTo("Need more values in order to deduce a solution."));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddSolutions()
        {
            // Arrange

            // Act
            var result = sutSuccess.AddSolutions(addSolutionRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void RejectAddSolutionRequestsOfMoreThanAThousand()
        {
            // Arrange

            // Act
            var result = sutSuccess.AddSolutions(invalidAddSolutionRequest);
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;
            var statusMessage = ((BadRequestObjectResult)result.Result).Value;
            var errorMessage = "The amount of solutions requested, 1001, exceeds the service's 1,000 solution limit";

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(400));
            Assert.That(statusMessage, Is.EqualTo(errorMessage));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldAddSolutionsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.AddSolutions(addSolutionRequest);
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
