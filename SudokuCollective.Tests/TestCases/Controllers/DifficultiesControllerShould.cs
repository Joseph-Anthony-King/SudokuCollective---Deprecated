using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class DifficultiesControllerShould
    {
        private DatabaseContext context;
        private DifficultiesController sutSuccess;
        private DifficultiesController sutFailure;
        private MockDifficultiesService mockDifficultiesService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private UpdateDifficultyRequest updateDifficultyRequest;
        private CreateDifficultyRequest createDifficultyRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockDifficultiesService = new MockDifficultiesService(context);
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            updateDifficultyRequest = new UpdateDifficultyRequest()
            {
                Id = 1,
                Name = "Test Difficulty",
                DifficultyLevel = DifficultyLevel.TEST,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            createDifficultyRequest = new CreateDifficultyRequest()
            {
                Name = "Test Difficulty",
                DifficultyLevel = DifficultyLevel.TEST,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sutSuccess = new DifficultiesController(
                mockDifficultiesService.DifficultiesServiceSuccessfulRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutFailure = new DifficultiesController(
                mockDifficultiesService.DifficultiesServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetDifficulty()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutSuccess.GetDifficulty(difficultyId, baseRequest);
            var difficulty = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(difficulty, Is.InstanceOf<Difficulty>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetDifficultyFail()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutFailure.GetDifficulty(difficultyId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving difficulty"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetDifficulties(baseRequest, true);
            var difficulties = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Difficulty>>>());
            Assert.That(((List<Difficulty>)difficulties).Count, Is.EqualTo(6));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetDifficultiesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetDifficulties(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Difficulty>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving difficulties"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.PutDifficulty(1, updateDifficultyRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PutDifficulty(1, updateDifficultyRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating difficulty"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.PostDifficulty(createDifficultyRequest);
            var difficulty = ((CreatedAtActionResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(difficulty, Is.InstanceOf<Difficulty>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateDifficultiesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PostDifficulty(createDifficultyRequest);
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteDifficulties()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutSuccess.DeleteDifficulty(difficultyId, baseRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteDifficultiesFail()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutFailure.DeleteDifficulty(difficultyId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deleting difficulty"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
