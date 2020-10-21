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

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AuthenticateControllerShould {

        private AuthenticateController sut;
        private AuthenticateController sutInvalid;
        private AuthenticateController sutInvalidUserName;
        private AuthenticateController sutInvalidPassword;
        private Mock<IAuthenticateService> mockAuthenticateService;
        private Mock<IAuthenticateService> mockValidAuthenticateService;
        private Mock<IAuthenticateService> mockInvalidAuthenticateService;
        private Mock<IUserManagementService> mockUserManagementService;
        private Mock<IUserManagementService> mockUserManagementInvalidUserNameService;
        private Mock<IUserManagementService> mockUserManagementInvalidPasswordService;
        private TokenRequest tokenRequest;
        private string userName;
        private string password;

        [SetUp]
        public void Setup() {

            userName = "TestSuperUser";
            password = "password1";

            mockAuthenticateService = new Mock<IAuthenticateService>();
            mockValidAuthenticateService = new Mock<IAuthenticateService>();
            mockInvalidAuthenticateService = new Mock<IAuthenticateService>();
            mockUserManagementService = new Mock<IUserManagementService>();
            mockUserManagementInvalidUserNameService = new Mock<IUserManagementService>();
            mockUserManagementInvalidPasswordService = new Mock<IUserManagementService>();

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
    }
}
