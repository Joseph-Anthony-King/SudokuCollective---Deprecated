using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.TokenModels;
using SudokuCollective.WebApi.Models.DTOModels;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Services.Interfaces;
using SudokuCollective.WebApi.Models.ResultModels.UserResults;
using SudokuCollective.WebApi.Models.ResultModels.AuthenticationResults;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AuthenticateControllerShould {

        private AuthenticateController sut;
        private AuthenticateController sutInvalid;
        private AuthenticateController sutInvalidUserName;
        private AuthenticateController sutInvalidPassword;
        private AuthenticateController sutUserNameFound;
        private AuthenticateController sutUserNameNotFound;
        private Mock<IAuthenticateService> mockAuthenticateService;
        private Mock<IAuthenticateService> mockValidAuthenticateService;
        private Mock<IAuthenticateService> mockInvalidAuthenticateService;
        private Mock<IUserManagementService> mockUserManagementService;
        private Mock<IUserManagementService> mockUserManagementInvalidUserNameService;
        private Mock<IUserManagementService> mockUserManagementInvalidPasswordService;
        private Mock<IUserManagementService> mockUserManagementUserNameFoundService;
        private Mock<IUserManagementService> mockUserManagementUserNameNotFoundService;
        private TokenRequest tokenRequest;
        private string userName;
        private string password;
        private string email;

        [SetUp]
        public void Setup() {

            userName = "TestSuperUser";
            password = "password1";
            email = "TestSuperUser@example.com";

            mockAuthenticateService = new Mock<IAuthenticateService>();
            mockValidAuthenticateService = new Mock<IAuthenticateService>();
            mockInvalidAuthenticateService = new Mock<IAuthenticateService>();
            mockUserManagementService = new Mock<IUserManagementService>();
            mockUserManagementInvalidUserNameService = new Mock<IUserManagementService>();
            mockUserManagementInvalidPasswordService = new Mock<IUserManagementService>();
            mockUserManagementUserNameFoundService = new Mock<IUserManagementService>();
            mockUserManagementUserNameNotFoundService = new Mock<IUserManagementService>();

            tokenRequest = new TokenRequest() {

                UserName = userName,
                Password = password
            };

            var token = string.Empty;
            var user = new AuthenticatedUser();

            mockValidAuthenticateService
                .Setup(authService => authService.IsAuthenticated(tokenRequest, out token, out user))
                .Returns(true);
            mockInvalidAuthenticateService
                .Setup(authService => authService.IsAuthenticated(tokenRequest, out token, out user))
                .Returns(false);
            mockUserManagementInvalidUserNameService
                .Setup(userManagementService => userManagementService
                    .ConfirmAuthenticationIssue(tokenRequest.UserName, tokenRequest.Password))
                .ReturnsAsync(UserAuthenticationErrorType.USERNAMEINVALID);
            mockUserManagementInvalidPasswordService
                .Setup(userManagementService => userManagementService
                    .ConfirmAuthenticationIssue(tokenRequest.UserName, tokenRequest.Password))
                .ReturnsAsync(UserAuthenticationErrorType.PASSWORDINVALID);
            mockUserManagementUserNameFoundService
                .Setup(userManagementService => userManagementService.ConfirmUserName(email))
                .ReturnsAsync(new AuthenticationResult() { Success = true, UserName = userName });
            mockUserManagementUserNameNotFoundService
                .Setup(userManagementService => userManagementService.ConfirmUserName(email))
                .ReturnsAsync(new AuthenticationResult() { Success = false, Message = "Email Does Not Exist" });

            sut = new AuthenticateController(
                mockValidAuthenticateService.Object, 
                mockUserManagementService.Object);
            sutInvalid = new AuthenticateController(
                mockInvalidAuthenticateService.Object, 
                mockUserManagementService.Object);
            sutInvalidUserName = new AuthenticateController(
                mockAuthenticateService.Object, 
                mockUserManagementInvalidUserNameService.Object);
            sutInvalidPassword = new AuthenticateController(
                mockAuthenticateService.Object, 
                mockUserManagementInvalidPasswordService.Object);
            sutUserNameFound = new AuthenticateController(
                mockAuthenticateService.Object,
                mockUserManagementUserNameFoundService.Object);
            sutUserNameNotFound = new AuthenticateController(
                mockAuthenticateService.Object,
                mockUserManagementUserNameNotFoundService.Object);
        }

        [Test]
        [Category("Controllers")]
        public void AuthenticateUsers() {

            // Arrange

            // Act
            var result = sut.RequestToken(tokenRequest);
            var returnedValue = ((OkObjectResult)result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(returnedValue, Is.TypeOf<AuthenticatedUserResult>());
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
        public void ReturnBadRequestMessageWhenUsersArentAuthenticated() {

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
        public void ReturnUserName() {

            // Arrange

            // Act
            var result = sutUserNameFound.ConfirmUserName(email);
            var username = ((OkObjectResult)result.Result).Value;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(username, Is.EqualTo(userName));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnErrorMessageIfUserNameNotFound() {

            // Arrange

            // Act
            var result = sutUserNameNotFound.ConfirmUserName(email);
            var message = ((BadRequestObjectResult)result.Result).Value;

            // Assert
            Assert.That(result, Is.TypeOf<Task<ActionResult>>());
            Assert.That(message, Is.EqualTo("Status Code 400: No Record Of Email Address"));
        }
    }
}
