using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Test.MockServices;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class AuthenticateControllerShould
    {
        private AuthenticateController sut;
        private AuthenticateController sutInvalid;
        private AuthenticateController sutInvalidUserName;
        private AuthenticateController sutInvalidPassword;
        private AuthenticateController sutUserNameNotFound;
        private MockAuthenticateService mockAuthenticateService;
        private MockUserManagementService mockUserManagementService;
        private MockUserManagementService mockUserManagementInvalidUserNameService;
        private MockUserManagementService mockUserManagementInvalidPasswordService;
        private TokenRequest tokenRequest;
        private string userName;
        private string password;
        private string email;

        [SetUp]
        public async Task Setup()
        {
            userName = "TestSuperUser";
            password = "password1";
            email = "TestSuperUser@example.com";

            mockAuthenticateService = new MockAuthenticateService();
            mockUserManagementService = new MockUserManagementService();
            mockUserManagementInvalidUserNameService = new MockUserManagementService();
            mockUserManagementInvalidPasswordService = new MockUserManagementService();

            tokenRequest = new TokenRequest()
            {
                UserName = userName,
                Password = password
            };

            sut = new AuthenticateController(
                mockAuthenticateService.AuthenticateServiceSuccessfulRequest.Object,
                mockUserManagementService.UserManagementServiceSuccssfulRequest.Object);
            sutInvalid = new AuthenticateController(
                mockAuthenticateService.AuthenticateServiceFailedRequest.Object,
                mockUserManagementService.UserManagementServiceFailedRequest.Object);
            sutInvalidUserName = new AuthenticateController(
                mockAuthenticateService.AuthenticateServiceFailedRequest.Object,
                mockUserManagementInvalidUserNameService.UserManagementServiceUserNameFailedRequest.Object);
            sutInvalidPassword = new AuthenticateController(
                mockAuthenticateService.AuthenticateServiceFailedRequest.Object,
                mockUserManagementInvalidPasswordService.UserManagementServicePasswordFailedRequest.Object);
            sutUserNameNotFound = new AuthenticateController(
                mockAuthenticateService.AuthenticateServiceFailedRequest.Object,
                mockUserManagementService.UserManagementServiceFailedRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void AuthenticateUsers()
        {
            // Arrange

            // Act
            var result = sut.RequestToken(tokenRequest);
            var returnedValue = ((OkObjectResult)result.Result).Value;
            var message = ((AuthenticatedUserResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(returnedValue, Is.TypeOf<AuthenticatedUserResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestMessageWhenUserNameIsInvalid()
        {
            // Arrange

            // Act
            var result = sutInvalidUserName.RequestToken(tokenRequest);
            var message = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 400: User Name Invalid"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestMessageWhenPasswordIsInvalid()
        {
            // Arrange

            // Act
            var result = sutInvalidPassword.RequestToken(tokenRequest);
            var message = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 400: Password Invalid"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnBadRequestMessageWhenUsersArentAuthenticated()
        {
            // Arrange

            // Act
            var result = sutInvalid.RequestToken(tokenRequest);
            var message = ((BadRequestObjectResult)result.Result).Value;
            var statusCode = ((BadRequestObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 400: Bad Request"));
            Assert.That(statusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnUserName()
        {
            // Arrange

            // Act
            var result = sut.ConfirmUserName(email);
            var message = ((AuthenticationResult)((OkObjectResult)result.Result).Value).Message;
            var username = ((AuthenticationResult)((OkObjectResult)result.Result).Value).UserName;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Name Confirmed"));
            Assert.That(username, Is.EqualTo(userName));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnErrorMessageIfUserNameNotFound()
        {
            // Arrange

            // Act
            var result = sutUserNameNotFound.ConfirmUserName(email);
            var message =((AuthenticationResult)((NotFoundObjectResult)result.Result).Value).Message;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Email Does Not Exist"));
        }
    }
}
