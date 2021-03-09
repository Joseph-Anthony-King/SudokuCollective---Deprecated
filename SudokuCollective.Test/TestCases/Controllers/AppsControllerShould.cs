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
        private AppUserRequest appUserRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            appRequest = TestObjects.GetAppRequest();

            appUserRequest = new AppUserRequest
            {
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                AppId = 1,
                TargetLicense = TestObjects.GetLicense(),
            };

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
            var result = sutSuccess.GetApp(appId, baseRequest);
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
            var result = sutFailure.GetApp(appId, baseRequest);
            var message = ((AppResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAppByLicense()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetAppByLicense(baseRequest, true);
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
            var result = sutFailure.GetAppByLicense(baseRequest, true);
            var message = ((AppResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetApps(baseRequest, true);
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
            var result = sutFailure.GetApps(baseRequest, true);
            var message = ((AppsResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Apps Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.UpdateApp(1, appRequest);
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
            var result = sutFailure.UpdateApp(1, appRequest);
            var message = ((AppResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Updated"));
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
                baseRequest, 
                true);
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
                baseRequest, 
                true);
            var message = ((UsersResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Users Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddUserToApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.AddUser(3, appUserRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Added To App"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyAddUserToAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.AddUser(3, appUserRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User Not Added To App"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRemoveUserFromApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.RemoveUser(3, appUserRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Removed From App"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyRemoveUserFromAppFail()
        {
            // Arrange

            // Act
            var result = sutFailure.RemoveUser(3, appUserRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User Not Removed From App"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateAnApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.ActivateApp(1);
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
            var result = sutFailure.ActivateApp(1);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Activated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateAnApp()
        {
            // Arrange

            // Act
            var result = sutSuccess.DeactivateApp(1);
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
            var result = sutFailure.DeactivateApp(1);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Deactivated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestResponseShouldLicenseValidationFail()
        {
            // Arrange
            var appId = 1;

            // Act
            var resultOne = sutInvalid.GetApp(appId, baseRequest);
            var messageOne = ((BadRequestObjectResult)resultOne.Result.Result).Value;
            var statusCodeOne = ((BadRequestObjectResult)resultOne.Result.Result).StatusCode;

            var resultTwo = sutInvalid.GetApps(baseRequest, true);
            var messageTwo = ((BadRequestObjectResult)resultTwo.Result.Result).Value;
            var statusCodeTwo = ((BadRequestObjectResult)resultTwo.Result.Result).StatusCode;

            var resultThree = sutInvalid.UpdateApp(1, appRequest);
            var messageThree = ((BadRequestObjectResult)resultThree.Result).Value;
            var statusCodeThree = ((BadRequestObjectResult)resultThree.Result).StatusCode;

            var resultFour = sutInvalid.GetAppUsers(1, baseRequest, true);
            var messageFour = ((BadRequestObjectResult)resultFour.Result.Result).Value;
            var statusCodeFour = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            var resultFive = sutInvalid.AddUser(3, appUserRequest);
            var messageFive = ((BadRequestObjectResult)resultFive.Result).Value;
            var statusCodeFive = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            var resultSix = sutInvalid.RemoveUser(3, appUserRequest);
            var messageSix = ((BadRequestObjectResult)resultSix.Result).Value;
            var statusCodeSix = ((BadRequestObjectResult)resultFour.Result.Result).StatusCode;

            // Assert
            Assert.That(resultOne.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(messageOne, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeOne, Is.EqualTo(400));
            Assert.That(resultTwo.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(messageTwo, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeTwo, Is.EqualTo(400));
            Assert.That(resultThree.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageThree, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeThree, Is.EqualTo(400));
            Assert.That(resultFour.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(messageFour, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeFour, Is.EqualTo(400));
            Assert.That(resultFive.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageFive, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeFive, Is.EqualTo(400));
            Assert.That(resultSix.Result, Is.InstanceOf<ActionResult>());
            Assert.That(messageSix, Is.EqualTo("Status Code 400: Invalid Request On This License"));
            Assert.That(statusCodeSix, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAllowSuperuserToDeleteApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.DeleteApp(2, baseRequest);
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
            var result = sutFailure.DeleteApp(2, baseRequest);
            var errorMessage = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.EqualTo("Status Code 400: You Are Not The Owner Of This App"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAllowAdminToDeleteApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.ResetApp(2, baseRequest);
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
            var result = sutFailure.ResetApp(2, baseRequest);
            var message = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 400: You Are Not The Owner Of This App"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyPromoteUserToAppAdmin()
        {
            // Arrange

            // Act
            var result = sutSuccess.ObtainAdminPrivileges(3, appUserRequest);
            var message = ((UserResult)((OkObjectResult)result.Result).Value).Message;
            var user = ((UserResult)((OkObjectResult)result.Result).Value).User;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Has Been Promoted To Admin"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyPromoteUserToAppAdminFail()
        {
            // Arrange

            // Act
            var result = sutPromoteUserFailure.ObtainAdminPrivileges(3, appUserRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User Has Not Been Promoted To Admin"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateAdminPrivileges()
        {
            // Arrange

            // Act
            var result = sutSuccess.ActivateAdminPrivileges(3, appUserRequest);
            var message = ((UserResult)((OkObjectResult)result.Result).Value).Message;
            var user = ((UserResult)((OkObjectResult)result.Result).Value).User;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Admin Privileges Activated"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyActivateAdminPrivilegesFail()
        {
            // Arrange

            // Act
            var result = sutPromoteUserFailure.ActivateAdminPrivileges(3, appUserRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Activation Of Admin Privileges Failed"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateAdminPrivileges()
        {
            // Arrange

            // Act
            var result = sutSuccess.DeactivateAdminPrivileges(3, appUserRequest);
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
            var result = sutPromoteUserFailure.DeactivateAdminPrivileges(3, appUserRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Deactivation Of Admin Privileges Failed"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetMyApps()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetMyApps(baseRequest, true);
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
            var result = sutFailure.GetMyApps(baseRequest, true);
            var message = ((AppsResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Apps Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetAccessTokenTimeFrames()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetTimeFrames(baseRequest);
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<List<TimeFrameListItem>>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnFalseIfSuccessfullyGetAccessTokenTimeFramesFails()
        {
            // Arrange

            // Act
            var result = sutInvalid.GetTimeFrames(baseRequest);
            var statusCode = ((BadRequestObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<List<TimeFrameListItem>>>());
            Assert.That(statusCode, Is.EqualTo(400));
        }
    }
}
