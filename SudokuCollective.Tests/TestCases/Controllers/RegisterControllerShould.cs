using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class RegisterControllerShould {

        private RegisterController sut;
        private Mock<IUsersService> mockUserService;
        private RegisterRequest registerRequest;

        [SetUp]
        public void Setup() {

            mockUserService = new Mock<IUsersService>();
            registerRequest = new RegisterRequest() {

                UserName = "Test User 3",
                FirstName = "Test",
                LastName = "User 3",
                NickName = "My Nickname",
                Email = "testuser3@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            mockUserService
                .Setup(userService => userService.CreateUser(registerRequest, true))
                .Returns(Task.FromResult(new UserResult() {

                    Success = true,
                    Message = string.Empty,
                    User = new User(
                        3, 
                        "Test User 3", 
                        "Test", 
                        "User 3", 
                        "My Nickname", 
                        "testuser3@example.com", 
                        "password1", 
                        true, 
                        DateTime.UtcNow, 
                        DateTime.UtcNow)
                }));

            sut = new RegisterController(mockUserService.Object);
        }

        [Test]
        [Category("Services")]
        public void RegisterUsers() {

            // Arrange

            // Act
            var result = sut.SignUp(registerRequest, true);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
        }
    }
}
