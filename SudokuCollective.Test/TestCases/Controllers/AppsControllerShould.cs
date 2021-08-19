using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models.ResultModels;
using static SudokuCollective.Api.V1.Controllers.AppsController;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class AppsControllerShould
    {
        private DatabaseContext context;
        private AppsController sutSuccess;
        private AppsController sutFailure;
        private AppsController sutInvalid;
        private AppsController sutPromoteUserFailure;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private AppRequest appRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            appRequest = TestObjects.GetAppRequest();

            sutSuccess = new AppsController(mockAppsService.AppsServiceSuccessfulRequest.Object);
            sutFailure = new AppsController(mockAppsService.AppsServiceFailedRequest.Object);
            sutInvalid = new AppsController(mockAppsService.AppsServiceInvalidRequest.Object);
            sutPromoteUserFailure = new AppsController(mockAppsService.AppsServicePromoteUserFailsRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetApp()
        {
            // Arrange
            var appId = 1;

            // Act
            var result = sutSuccess.Get(appId, baseRequest);
            var app = ((AppResult)((OkObjectResult)result.Result.Result).Value).App;
            var message = ((AppResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(((App)app).Id, Is.EqualTo(1));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetAppFail()
        {
            // Arrange
            var appId = 1;

            // Act
            var result = sutFailure.Get(appId, baseRequest);
            var message = ((AppResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 404: App not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAppByLicense()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetByLicense(baseRequest);
            var app = ((AppResult)((OkObjectResult)result.Result.Result).Value).App;
            var message = ((AppResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Found"));
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetAppByLicenseFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetByLicense(baseRequest);
            var message = ((AppResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 404: App not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetApps(baseRequest);
            var apps = ((AppsResult)((OkObjectResult)result.Result.Result).Value).Apps;
            var message = ((AppsResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Apps Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(apps, Is.InstanceOf<List<IApp>>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetApps(baseRequest);
            var message = ((AppsResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Apps not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.Update(1, appRequest);
            var message = ((AppResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Updated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyUpdateAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Update(1, appRequest);
            var message = ((AppResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAppUsers()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetAppUsers(
                1,
                baseRequest);
            var message = ((UsersResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var users = ((UsersResult)((OkObjectResult)result.Result.Result).Value).Users;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Users Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetAppUsersFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetAppUsers(
                1,
                baseRequest);
            var message = ((UsersResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Users not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddUserToApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.AddUser(1, 3, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Added to App"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyAddUserToAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.AddUser(1, 3, baseRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Added to App"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRemoveUserFromApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.RemoveUser(1, 3, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Removed from App"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyRemoveUserFromAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.RemoveUser(1, 3, baseRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Removed from App"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateAnApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.Activate(1);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Activated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyActivateAnAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Activate(1);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App not Activated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateAnApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.Deactivate(1);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Deactivated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyDeactivateAnAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Deactivate(1);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App not Deactivated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestResponseShouldLicenseValidationFail()
        {
            // Arrange
            var appId = 1;

            // Act
            var resultOne = sutInvalid.Get(appId, baseRequest);
            var messageOne = ((BadRequestObjectResult)resultOne.Result.Result).Value;
            var statusCodeOne = ((BadRequestObjectResult)resultOne.Result.Result).StatusCode;

            var resultTwo = sutInvalid.GetApps(baseRequest);
            var messageTwo = ((BadRequestObjectResult)resultTwo.Result.Result).Value;
            var statusCodeTwo = ((BadRequestObjectResult)resultTwo.Result.Result).StatusCode;

            var resultThree = sutInvalid.Update(1, appRequest);
            var messageThree = ((BadRequestObjectResult)resultThree.Result).Value;
            var statusCodeThree = ((BadRequestObjectResult)resultThree.Result).StatusCode;

            var resultFour = sutInvalid.GetAppUsers(1, baseRequest);
            var messageFour = ((BadRequestObjectResult)resultFour.Result.Result).Value;
            var statusCodeFour = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            var resultFive = sutInvalid.AddUser(1, 3, baseRequest);
            var messageFive = ((BadRequestObjectResult)resultFive.Result).Value;
            var statusCodeFive = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            var resultSix = sutInvalid.RemoveUser(1, 3, baseRequest);
            var messageSix = ((BadRequestObjectResult)resultSix.Result).Value;
            var statusCodeSix = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            // Assert
            Assert.That(resultOne.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(messageOne, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeOne, Is.EqualTo(400));
            Assert.That(resultTwo.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(messageTwo, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeTwo, Is.EqualTo(400));
            Assert.That(resultThree.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageThree, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeThree, Is.EqualTo(400));
            Assert.That(resultFour.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(messageFour, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeFour, Is.EqualTo(400));
            Assert.That(resultFive.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageFive, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeFive, Is.EqualTo(400));
            Assert.That(resultSix.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageSix, Is.EqualTo("Status Code 400: Invalid Request on this License"));
            Assert.That(statusCodeSix, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAllowSuperuserToDeleteApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.Delete(2, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyAllowSuperuserToDeleteAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Delete(2, baseRequest);
            var errorMessage = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.EqualTo("Status Code 400: You are not the Owner of this App"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAllowAdminToDeleteApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.Reset(2, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyAllowAdminToDeleteAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.Reset(2, baseRequest);
            var message = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 400: You are not the Owner of this App"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyPromoteUserToAppAdmin()
        {
            // Arrange

            // Act
            var result = sutSuccess.ActivateAdminPrivileges(1, 3, baseRequest);
            var message = ((UserResult)((OkObjectResult)result.Result).Value).Message;
            var user = ((UserResult)((OkObjectResult)result.Result).Value).User;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User has been Promoted to Admin"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyPromoteUserToAppAdminFail()
        {
            // Arrange

            // Act
            var result = sutPromoteUserFailure.ActivateAdminPrivileges(1, 3, baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User has not been Promoted to Admin"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateAdminPrivileges()
        {
            // Arrange

            // Act
            var result = sutSuccess.DeactivateAdminPrivileges(1, 3, baseRequest);
            var message = ((UserResult)((OkObjectResult)result.Result).Value).Message;
            var user = ((UserResult)((OkObjectResult)result.Result).Value).User;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Admin Privileges Deactivated"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyDeactivateAdminPrivilegesFail()
        {
            // Arrange

            // Act
            var result = sutPromoteUserFailure.DeactivateAdminPrivileges(1, 3, baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Deactivation of Admin Privileges Failed"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetMyApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetMyApps(baseRequest);
            var apps = ((AppsResult)((OkObjectResult)result.Result.Result).Value).Apps;
            var message = ((AppsResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Apps Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(apps, Is.InstanceOf<List<IApp>>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetMyAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetMyApps(baseRequest);
            var message = ((AppsResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Apps not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetRegisteredApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.RegisteredApps(2, baseRequest);
            var apps = ((AppsResult)((OkObjectResult)result.Result).Value).Apps;
            var message = ((AppsResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Apps Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(apps, Is.InstanceOf<List<IApp>>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyGetRegisteredAppsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.RegisteredApps(1, baseRequest);
            var message = ((AppsResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Apps not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAccessTokenTimeFrames()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetTimeFrames();
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<EnumListItem>>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetSortValues()
        {
            // Arrange

            // Act
            var result = sutSuccess.sortValues();
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<EnumListItem>>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
