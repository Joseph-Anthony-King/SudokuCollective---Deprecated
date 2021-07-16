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
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Data.Models.ResultModels;

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
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
            };

            createDifficultyRequest = new CreateDifficultyRequest()
            {
                Name = "Test Difficulty",
                DifficultyLevel = DifficultyLevel.TEST,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
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
            var result = sutSuccess.Get(difficultyId);
            var message = ((DifficultyResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var difficulty = ((DifficultyResult)((OkObjectResult)result.Result.Result).Value).Difficulty;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Difficulty Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(difficulty, Is.InstanceOf<Difficulty>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetDifficultyFail()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutFailure.Get(difficultyId);
            var message = ((DifficultyResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Difficulty not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetDifficulties();
            var message = ((DifficultiesResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var difficulties = ((DifficultiesResult)((OkObjectResult)result.Result.Result).Value).Difficulties;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Difficulty>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Difficulties Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(difficulties.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetDifficultiesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetDifficulties();
            var message = ((DifficultiesResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Difficulty>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Difficulties not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.Update(1, updateDifficultyRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Difficulty Updated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateGamesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Update(1, updateDifficultyRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Difficulty not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateDifficulties()
        {
            // Arrange

            // Act
            var result = sutSuccess.Post(createDifficultyRequest);
            var message = ((DifficultyResult)((ObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result.Result).StatusCode;
            var difficulty = ((DifficultyResult)((ObjectResult)result.Result.Result).Value).Difficulty;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(message, Is.EqualTo("Status Code 201: Difficulty Created"));
            Assert.That(statusCode, Is.EqualTo(201));
            Assert.That(difficulty, Is.InstanceOf<Difficulty>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateDifficultiesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Post(createDifficultyRequest);
            var message = ((DifficultyResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Difficulty>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Difficulty not Created"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteDifficulties()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutSuccess.Delete(difficultyId, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Difficulty Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteDifficultiesFail()
        {
            // Arrange
            var difficultyId = 2;

            // Act
            var result = sutFailure.Delete(difficultyId, baseRequest);
            var message = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Difficulty not Deleted"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
