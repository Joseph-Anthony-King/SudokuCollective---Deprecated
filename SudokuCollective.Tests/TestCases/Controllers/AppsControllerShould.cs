using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.MockServices;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AppsControllerShould {

        private DatabaseContext context;
        private AppsController sutSuccess;
        private AppsController sutFailure;
        private AppsController sutInvalid;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private AppRequest appRequest;

        [SetUp]
        public async Task Setup() {

            context = await TestDatabase.GetDatabaseContext();
            baseRequest = new BaseRequest();
            mockAppsService = new MockAppsService(context);

            appRequest = new AppRequest() {

                Name = "New Test App 3",
                DevUrl = "https://localhost:8080",
                LiveUrl = "https://TestApp3.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sutSuccess = new AppsController(mockAppsService.AppsServiceSuccessfulRequest.Object);
            sutFailure = new AppsController(mockAppsService.AppsServiceFailedRequest.Object);
            sutInvalid = new AppsController(mockAppsService.AppsServiceInvalidRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetApp() {

            // Arrange
            var appId = 1;

            // Act
            var result = sutSuccess.GetApp(appId, baseRequest);
            var app = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(((App)app).Id, Is.EqualTo(1));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetAppFail() {

            // Arrange
            var appId = 1;

            // Act
            var result = sutFailure.GetApp(appId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error getting app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAppByLicense() {

            // Arrange

            // Act
            var result = sutSuccess.GetAppByLicense(baseRequest, true);
            var app = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetAppByLicenseFail() {

            // Arrange

            // Act
            var result = sutFailure.GetAppByLicense(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error getting app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetApps() {

            // Arrange

            // Act
            var result = sutSuccess.GetApps(baseRequest, true);
            var apps = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(apps, Is.InstanceOf<List<App>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetAppsFail() {

            // Arrange

            // Act
            var result = sutFailure.GetApps(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error getting apps"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateApps() {

            // Arrange

            // Act
            var result = sutSuccess.UpdateApp(appRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyUpdateAppsFail() {

            // Arrange

            // Act
            var result = sutFailure.UpdateApp(appRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAppUsers() {

            // Arrange

            // Act
            var result = sutSuccess.GetUsers(baseRequest, true);
            var users = ((OkObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(((List<User>)users).Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetAppUsersFail() {

            // Arrange

            // Act
            var result = sutFailure.GetUsers(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving app users"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddUserToApp() {

            // Arrange

            // Act
            var result = sutSuccess.AddUser(3, baseRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyAddUserToAppFail() {

            // Arrange

            // Act
            var result = sutFailure.AddUser(3, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error adding user to app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRemoveUserFromApp() {

            // Arrange

            // Act
            var result = sutSuccess.RemoveUser(3, baseRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyRemoveUserFromAppFail() {

            // Arrange

            // Act
            var result = sutFailure.RemoveUser(3, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error removing user from app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateAnApp() {

            // Arrange

            // Act
            var result = sutSuccess.ActivateApp(1);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyActivateAnAppFail() {

            // Arrange

            // Act
            var result = sutFailure.ActivateApp(1);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error activating app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateAnApp() {

            // Arrange

            // Act
            var result = sutSuccess.DeactivateApp(1);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyDeactivateAnAppFail() {

            // Arrange

            // Act
            var result = sutFailure.DeactivateApp(1);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deactivating app"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestResponseShouldLicenseValidationFail() {

            // Arrange
            var appId = 1;

            // Act
            var resultOne = sutInvalid.GetApp(appId, baseRequest);
            var statusCodeOne = ((BadRequestObjectResult)resultOne.Result.Result).StatusCode;

            var resultTwo = sutInvalid.GetAppByLicense(baseRequest, true);
            var statusCodeTwo = ((BadRequestObjectResult)resultTwo.Result.Result).StatusCode;

            var resultThree = sutInvalid.GetApps(baseRequest, true);
            var statusCodeThree = ((BadRequestObjectResult)resultThree.Result.Result).StatusCode;
            
            var resultFour = sutInvalid.UpdateApp(appRequest);
            var statusCodeFour = ((BadRequestObjectResult)resultFour.Result).StatusCode;
            
            var resultFive = sutInvalid.GetUsers(baseRequest, true);
            var statusCodeFive = ((BadRequestObjectResult)resultFive.Result.Result).StatusCode;
            
            var resultSix = sutInvalid.AddUser(3, baseRequest);
            var statusCodeSix = ((BadRequestObjectResult)resultFive.Result.Result).StatusCode;
            
            var resultSeven = sutInvalid.RemoveUser(3, baseRequest);
            var statusCodeSeven = ((BadRequestObjectResult)resultFive.Result.Result).StatusCode;

            // Assert
            Assert.That(resultOne.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(statusCodeOne, Is.EqualTo(400));
            Assert.That(resultTwo.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(statusCodeTwo, Is.EqualTo(400));
            Assert.That(resultFour.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCodeFour, Is.EqualTo(400));
            Assert.That(resultFive.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(statusCodeFive, Is.EqualTo(400));
            Assert.That(resultSeven.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCodeSeven, Is.EqualTo(400));

        }
    }
}
