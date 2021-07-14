﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class UsersControllerShould
    {
        private DatabaseContext context;
        private UsersController sutSuccess;
        private UsersController sutFailure;
        private MockUsersService mockUsersService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private UpdateUserRequest updateUserRequest;
        private UpdatePasswordRequest updatePasswordRequest;
        private UpdateUserRoleRequest updateUserRoleRequest;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockUsersService = new MockUsersService(context);
            mockAppsService = new MockAppsService(context);
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            baseRequest = new BaseRequest();

            updateUserRequest = new UpdateUserRequest()
            {
                UserName = "Test Username",
                FirstName = "FirstName",
                LastName = "LastName",
                NickName = "MyNickname",
                Email = "testemail@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
            };

            updatePasswordRequest = new UpdatePasswordRequest()
            {
                UserId = 1,
                NewPassword = "password2"
            };

            updateUserRoleRequest = new UpdateUserRoleRequest()
            {
                RoleIds = new List<int>() { 3 },
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
            };

            sutSuccess = new UsersController(
                mockUsersService.UsersServiceSuccessfulRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object,
                mockWebHostEnvironment.Object);

            sutFailure = new UsersController(
                mockUsersService.UsersServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object,
                mockWebHostEnvironment.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetUser()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutSuccess.GetUser(userId, baseRequest, true);
            var message = ((UserResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var user = ((UserResult)((OkObjectResult)result.Result.Result).Value).User;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetUserFail()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = sutFailure.GetUser(userId, baseRequest, true);
            var message = ((UserResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetUsers()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetUsers(baseRequest);
            var message = ((UsersResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Users Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetUsersFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetUsers(baseRequest);
            var message = ((UsersResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Users not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateUsers()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.UpdateUser(userId, updateUserRequest);
            var message = ((UserResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Updated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateUserFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.UpdateUser(userId, updateUserRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateUsersPasswords()
        {
            // Arrange
            var updatePasswordRequest = TestObjects.GetRequestPasswordResetRequest();

            // Act
            var result = sutSuccess.RequestPasswordReset(updatePasswordRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Processed Password Reset Request"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateUsersPasswordsFail()
        {
            // Arrange
            var updatePasswordRequest = TestObjects.GetRequestPasswordResetRequest();

            // Act
            var result = sutFailure.RequestPasswordReset(updatePasswordRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Unable to Process Password Reset Request"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteUsers()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.DeleteUser(userId, baseRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Deleted"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteUsersFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.DeleteUser(userId, baseRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Deleted"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyAddUsersRole()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.AddRoles(userId, updateUserRoleRequest);
            var message = ((RolesResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Roles Added"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldAddUsersRoleFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.AddRoles(userId, updateUserRoleRequest);
            var message = ((RolesResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Roles not Added"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRemoveUsersRoles()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.RemoveRoles(userId, updateUserRoleRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Roles Removed"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldRemoveUsersRolesFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.RemoveRoles(userId, updateUserRoleRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Roles not Removed"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyActivateUsers()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.ActivateUser(userId);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Activated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldActivateUsersFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.ActivateUser(userId);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Activated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeactivateUsers()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutSuccess.DeactivateUser(userId);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: User Deactivated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeactivateUsersFail()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = sutFailure.DeactivateUser(userId);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Deactivated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyResendPasswordResetEmails()
        {
            // Arrange

            // Act
            var result = sutSuccess.ResendRequestPasswordReset(baseRequest);
            var message = ((BaseResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Password Reset Email Resent"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyResendEmailConfirmationFail()
        {
            // Arrange

            // Act
            var result = sutFailure.ResendRequestPasswordReset(baseRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: User not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCancelEmailConfirmation()
        {
            // Arrange

            // Act
            var result = sutSuccess.CancelEmailConfirmation(baseRequest);
            var message = ((UserResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Email Confirmation Request Cancelled"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyCancelEmailConfirmationFail()
        {
            // Arrange

            // Act
            var result = sutFailure.CancelEmailConfirmation(baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Email Confirmation Request not Cancelled"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCancelPasswordRequest()
        {
            // Arrange

            // Act
            var result = sutSuccess.CancelPasswordReset(baseRequest);
            var message = ((UserResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Password Reset Request Cancelled"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyCancelPasswordRequestFail()
        {
            // Arrange

            // Act
            var result = sutFailure.CancelPasswordReset(baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Password Reset Request not Cancelled"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCancelAllEmailRequests()
        {
            // Arrange

            // Act
            var result = sutSuccess.CancelAllEmailRequests(baseRequest);
            var message = ((UserResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Email Confirmation Request Cancelled and Password Reset Request Cancelled"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyCancelAllEmailRequestsFail()
        {
            // Arrange

            // Act
            var result = sutFailure.CancelAllEmailRequests(baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Email Requests not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
