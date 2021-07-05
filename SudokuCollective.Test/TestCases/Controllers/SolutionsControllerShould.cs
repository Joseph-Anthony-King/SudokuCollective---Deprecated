using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.ResultModels;

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
                FirstRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SecondRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                ThirdRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                FourthRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                FifthRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SixthRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                SeventhRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                EighthRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                NinthRow = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
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
            var message = ((SolutionResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var solution = ((SolutionResult)((OkObjectResult)result.Result.Result).Value).Solution;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Solution Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(solution, Is.InstanceOf<SudokuSolution>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetSolutionFail()
        {
            // Arrange
            var solutionId = 2;

            // Act
            var result = sutFailure.GetSolution(solutionId, baseRequest);
            var message = ((SolutionResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Solution Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetSolutions()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetSolutions(baseRequest, true);
            var message = ((SolutionsResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<SudokuSolution>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Solutions Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetSolutionsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetSolutions(baseRequest, true);
            var message = ((SolutionsResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<SudokuSolution>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Solutions Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullySolveSolution()
        {
            // Arrange

            // Act
            var result = sutSuccess.Solve(solveRequest);
            var message = ((SolutionResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Sudoku Solution Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSolveSolutionFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Solve(solveRequest);
            var message = ((SolutionResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Sudoku Solution Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGenerateSolution()
        {
            // Arrange

            // Act
            var result = sutSuccess.Generate();
            var message = ((SolutionResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Solution Generated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGenerateSolutionFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Generate();
            var message = ((SolutionResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Solution Not Generated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void IssueMessageIfSudokuSolutionSolveFailed()
        {
            // Arrange

            // Act
            var result = sutSolvedFailure.Solve(solveRequest);
            var message = ((SolutionResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<SudokuSolution>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Sudoku Solution Not Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddSolutions()
        {
            // Arrange

            // Act
            var result = sutSuccess.AddSolutions(addSolutionRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Solutions Added"));
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
            var message = ((BadRequestObjectResult)result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 400: The Amount Of Solutions Requested, 1001, Exceeds The Service's 1,000 Limit"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldAddSolutionsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.AddSolutions(addSolutionRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Solutions Not Added"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
