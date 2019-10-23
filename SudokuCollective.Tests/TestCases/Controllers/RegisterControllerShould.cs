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

        private RegisterController sutValid;
        private RegisterController sutInvalid;
        private Mock<IUsersService> mockUserServiceValid;
        private Mock<IUsersService> mockUserServiceInvalid;
        private RegisterRequest registerRequest;

        [SetUp]
        public void Setup() {

            mockUserServiceValid = new Mock<IUsersService>();
            mockUserServiceInvalid = new Mock<IUsersService>();

            mockUserServiceValid
                .Setup(userService => userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
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

            mockUserServiceInvalid
                .Setup(userService => userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = false,
                    Message = "Error creating user",
                    User = new User(
                        0, 
                        string.Empty,
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        false, 
                        DateTime.MinValue, 
                        DateTime.MinValue)
                }));

            sutValid = new RegisterController(mockUserServiceValid.Object);
            sutInvalid = new RegisterController(mockUserServiceInvalid.Object);

            registerRequest = new RegisterRequest() {                

                UserName = "TestUser3",
                FirstName = "Test",
                LastName = "User 3",
                NickName = "My Nickname",
                Email = "testuser3@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };
        }

        [Test]
        [Category("Controllers")]
        public void RegisterUsers() {

            // Arrange

            // Act
            var result = sutValid.SignUp(registerRequest, true);
            var taskResult = result.Result;
            var user = ((CreatedAtActionResult)taskResult.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void ProduceMessageWhenCreateUserGeneratesError() {

            // Arrange

            // Act
            var result = sutInvalid.SignUp(registerRequest, true);
            var taskResult = result.Result;
            var errorMessage = ((NotFoundObjectResult)taskResult.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error creating user"));
        }
    }
}
