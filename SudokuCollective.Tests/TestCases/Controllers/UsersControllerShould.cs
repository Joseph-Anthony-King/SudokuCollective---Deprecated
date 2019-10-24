using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.MockServices;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers
{

    public class UsersControllerShould {

        private DatabaseContext context;
        private UsersController sut;
        private MockUsersService mockUsersService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private UpdateUserRequest updateUserRequest;
        private UpdatePasswordRequest updatePasswordRequest;
        private UpdateUserRoleRequest updateUserRoleRequest;

        [SetUp]
        public async Task Setup() {

            context = await TestDatabase.GetDatabaseContext();
            mockUsersService = new MockUsersService(context);
            mockAppsService = new MockAppsService(context);
            
            baseRequest = new BaseRequest();

            updateUserRequest = new UpdateUserRequest() {

                UserName = "Test Username",
                FirstName = "FirstName",
                LastName = "LastName",
                NickName = "MyNickname",
                Email = "testemail@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            updatePasswordRequest = new UpdatePasswordRequest() {

                OldPassword = "password1",
                NewPassword = "password2",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            updateUserRoleRequest = new UpdateUserRoleRequest() {

                RoleIds = new List<int>() { 3 },
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sut = new UsersController(
                mockUsersService.UsersServiceSuccessfulRequest.Object, 
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void GetAUser() {

            // Arrange
            var userId = 1;

            // Act
            var result = sut.GetUser(userId, baseRequest, true);
            var user = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(user, Is.InstanceOf<User>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void GetUsers() {

            // Arrange

            // Act
            var result = sut.GetUsers(baseRequest);
            var users = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(((List<User>)users).Count, Is.EqualTo(2));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void UpdateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.PutUser(userId, updateUserRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void UpdateUsersPasswords() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.UpdatePassword(userId, updatePasswordRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void DeleteUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.DeleteUser(userId, baseRequest);
            var statusCode = ((OkResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void AddRolesToUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.AddRoles(userId, updateUserRoleRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void RemoveRolesFromUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.RemoveRoles(userId, updateUserRoleRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void ActivateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.ActivateUser(userId);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void DeactivateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sut.DeactivateUser(userId);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
