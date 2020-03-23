using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.TokenModels;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AuthenticateControllerShould {

        private AuthenticateController sut;
        private AuthenticateController sutInvalid;
        private Mock<IAuthenticateService> mockValidAuthenticateService;
        private Mock<IAuthenticateService> mockInvalidAuthenticateService;
        private TokenRequest tokenRequest;
        private string userName;
        private string password;

        [SetUp]
        public void Setup() {

            userName = "TestSuperUser";
            password = "password1";

            mockValidAuthenticateService = new Mock<IAuthenticateService>();
            mockInvalidAuthenticateService = new Mock<IAuthenticateService>();

            tokenRequest = new TokenRequest() {

                UserName = userName,
                Password = password
            };

            var token = string.Empty;

            mockValidAuthenticateService
                .Setup(authService => authService.IsAuthenticated(tokenRequest, out token))
                .Returns(true);
            mockInvalidAuthenticateService
                .Setup(authService => authService.IsAuthenticated(tokenRequest, out token))
                .Returns(false);

            sut = new AuthenticateController(mockValidAuthenticateService.Object);
            sutInvalid = new AuthenticateController(mockInvalidAuthenticateService.Object);
        }

        [Test]
        [Category("Controllers")]
        public void AuthenticateUsers() {

            // Arrange

            // Act
            var result = sut.RequestToken(tokenRequest);
            var statusCode = ((OkObjectResult)result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void ReturnMessageWhenUsersArentAuthenticated() {

            // Arrange

            // Act
            var result = sutInvalid.RequestToken(tokenRequest);
            var statusCode = ((BadRequestObjectResult)result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(statusCode, Is.EqualTo(400));
        }
    }
}
