using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.MockServices;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class UsersControllerShould {

        private DatabaseContext context;
        private UsersController sutSuccess;
        private UsersController sutFailure;
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

            sutSuccess = new UsersController(
                mockUsersService.UsersServiceSuccessfulRequest.Object, 
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutFailure = new UsersController(
                mockUsersService.UsersServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetUser() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.GetUser(userId, baseRequest, true);
            var user = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(user, Is.InstanceOf<User>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetUserFail() {

            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.GetUser(userId, baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetUsers() {

            // Arrange

            // Act
            var result = sutSuccess.GetUsers(baseRequest);
            var users = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(((List<User>)users).Count, Is.EqualTo(2));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetUsersFail() {

            // Arrange

            // Act
            var result = sutFailure.GetUsers(baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving users"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.PutUser(userId, updateUserRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateUsersFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.PutUser(userId, updateUserRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateUsersPasswords() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.UpdatePassword(userId, updatePasswordRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateUsersPasswordsFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.UpdatePassword(userId, updatePasswordRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating user password"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.DeleteUser(userId, baseRequest);
            var statusCode = ((OkResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteUsersFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.DeleteUser(userId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deleting user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddUsersRole() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.AddRoles(userId, updateUserRoleRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldAddUsersRoleFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.AddRoles(userId, updateUserRoleRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error adding role to user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRemoveUsersRoles() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.RemoveRoles(userId, updateUserRoleRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldRemoveUsersRolesFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.RemoveRoles(userId, updateUserRoleRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error removing role from user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.ActivateUser(userId);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldActivateUsersFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.ActivateUser(userId);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error activating user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateUsers() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.DeactivateUser(userId);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeactivateUsersFail() {

            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.DeactivateUser(userId);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deactivating user"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
