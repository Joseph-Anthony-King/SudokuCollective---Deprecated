using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.TokenModels;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AuthenticateControllerShould {

        private AuthenticateController sut;
        private Mock<IAuthenticateService> mockAuthenticateService;
        private Mock<IUserManagementService> mockUserManagementService;
        private TokenRequest tokenRequest;
        private string userName;
        private string password;

        [SetUp]
        public void Setup() {

            userName = "TestSuperUser";
            password = "password1";

            mockAuthenticateService = new Mock<IAuthenticateService>();

            tokenRequest = new TokenRequest() {

                UserName = userName,
                Password = password
            };

            var token = string.Empty;

            mockAuthenticateService.Setup(authService => authService.IsAuthenticated(tokenRequest, out token)).Returns(true);

            mockUserManagementService = new Mock<IUserManagementService>();
            mockUserManagementService.Setup(service => service.IsValidUser(userName, password)).Returns(Task.FromResult(true));

            sut = new AuthenticateController(mockAuthenticateService.Object);
        }

        [Test]
        [Category("Services")]
        public void AuthenticateUsers() {

            // Arrange

            // Act
            var result = sut.RequestToken(tokenRequest);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}
