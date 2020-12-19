using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class UsersServiceShould
    {
        private DatabaseContext context;
        private MockEmailService MockEmailService; 
        private MockUsersRepository MockUsersRepository;
        private MockAppsRepository MockAppsRepository;
        private MockRolesRepository MockRolesRepository;
        private MockEmailConfirmationsRepository MockEmailConfirmationsRepository;
        private MockPasswordUpdatesRepository MockPasswordUpdatesRepository;
        private IUsersService sut;
        private IUsersService sutFailure;
        private IUsersService sutEmailFailure;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            MockEmailService = new MockEmailService();
            MockUsersRepository = new MockUsersRepository(context);
            MockAppsRepository = new MockAppsRepository(context);
            MockRolesRepository = new MockRolesRepository(context);
            MockEmailConfirmationsRepository = new MockEmailConfirmationsRepository(context);
            MockPasswordUpdatesRepository = new MockPasswordUpdatesRepository(context);

            sut = new UsersService(
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object,
                MockEmailConfirmationsRepository.EmailConfirmationsRepositorySuccessfulRequest.Object,
                MockPasswordUpdatesRepository.PasswordUpdatesRepositorySuccessfulRequest.Object,
                MockEmailService.EmailServiceSuccessfulRequest.Object);

            sutFailure = new UsersService(
                MockUsersRepository.UsersRepositoryFailedRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object,
                MockEmailConfirmationsRepository.EmailConfirmationsRepositoryFailedRequest.Object,
                MockPasswordUpdatesRepository.PasswordUpdatesRepositorySuccessfulRequest.Object,
                MockEmailService.EmailServiceSuccessfulRequest.Object);

            sutEmailFailure = new UsersService(
                MockUsersRepository.UsersRepositoryEmailFailedRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object,
                MockEmailConfirmationsRepository.EmailConfirmationsRepositoryFailedRequest.Object,
                MockPasswordUpdatesRepository.PasswordUpdatesRepositorySuccessfulRequest.Object,
                MockEmailService.EmailServiceSuccessfulRequest.Object);

            baseRequest = TestObjects.GetBaseRequest();
        }

        [Test]
        [Category("Services")]
        public async Task GetUser()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await sut.GetUser(userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Found"));
            Assert.That(result.User, Is.TypeOf<User>());
        }

        [Test]
        [Category("Services")]
        public async Task ReturnMessageIfUserNotFound()
        {
            // Arrange
            var userId = 5;

            // Act
            var result = await sutFailure.GetUser(userId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Not Found"));
            Assert.That(result.User, Is.TypeOf<User>());
        }

        [Test]
        [Category("Services")]
        public async Task GetUsers()
        {
            // Arrange

            // Act
            var result = await sut.GetUsers(baseRequest.PageListModel);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Users Found"));
            Assert.That(result.Users, Is.TypeOf<List<IUser>>());
        }

        [Test]
        [Category("Services")]
        public async Task CreateUser()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/create-email-inlined.html";

            // Act
            var result = await sut.CreateUser(registerRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Created"));
            Assert.That(result.User, Is.TypeOf<User>());
        }

        [Test]
        [Category("Services")]
        public async Task ConfirmUserEmail()
        {
            // Arrange
            var emailConfirmation = context.EmailConfirmations.FirstOrDefault();

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sut.ConfirmEmail(emailConfirmation.Token, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Email Confirmed"));
        }

        [Test]
        [Category("Services")]
        public async Task NotifyIfConfirmUserEmailFails()
        {
            // Arrange
            var emailConfirmation = TestObjects.GetNewEmailConfirmation();

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sutEmailFailure.ConfirmEmail(emailConfirmation.Token, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email Not Confirmed"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserNameUnique()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "TestUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "c:/path/to/html";

            // Act
            var result = await sutFailure.CreateUser(registerRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Name Not Unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserName()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = null,
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "c:/path/to/html";

            // Act
            var result = await sutEmailFailure.CreateUser(registerRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Name Required"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueEmail()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "TestUser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var emailMetaData = new EmailMetaData();

            var html = "c:/path/to/html";

            // Act
            var result = await sutEmailFailure.CreateUser(registerRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email Not Unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireEmail()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = null,
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "c:/path/to/html";

            // Act
            var result = await sut.CreateUser(registerRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email Required"));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateUser()
        {
            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/create-email-inlined.html";

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Updated"));
            Assert.That(result.User.UserName, Is.EqualTo("TestSuperUserUPDATED"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueUserNameForUpdates()
        {
            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "TestUser",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sutFailure.UpdateUser(userId, updateUserRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Name Not Unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserNameForUpdates()
        {
            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = null,
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Name Required"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueEmailWithUpdates()
        {
            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sutEmailFailure.UpdateUser(userId, updateUserRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email Not Unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireEmailWithUpdates()
        {
            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = null,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var baseUrl = "https://example.com";

            var html = "../../../../SudokuCollective.Api/Content/EmailTemplates/confirm-old-email-inlined.html";

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest, baseUrl, html);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email Required"));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateUserPassword()
        {
            // Arrange
            var user = context.Users.FirstOrDefault(u => u.Id == 1);
            user.ReceivedRequestToUpdatePassword = true;
            context.SaveChanges();

            var updatePasswordRequest = new UpdatePasswordRequest()
            {
                UserId = user.Id,
                OldPassword = "password1",
                NewPassword = "password2",
            };

            // Act
            var result = await sut.UpdatePassword(updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Password Updated"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireOldPaswordForUpdateRequests()
        {
            // Arrange
            var user = context.Users.FirstOrDefault(u => u.Id == 1);
            user.ReceivedRequestToUpdatePassword = true;
            context.SaveChanges();

            var updatePasswordRequest = new UpdatePasswordRequest()
            {
                UserId = user.Id,
                OldPassword = "password!",
                NewPassword = "password2"
            };

            // Act
            var result = await sut.UpdatePassword(updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Old Password Incorrect"));
        }

        [Test]
        [Category("Services")]
        public async Task RejectPasswordUpdatesIfUserNotFound()
        {
            // Arrange
            var updatePasswordRequest = new UpdatePasswordRequest()
            {
                UserId = 1,
                OldPassword = "password1",
                NewPassword = "password2"
            };

            // Act
            var result = await sutFailure.UpdatePassword(updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteUsers()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await sut.DeleteUser(userId);
            var users = context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task ReturnErrorMessageIfUserNotFoundForDeletion()
        {
            // Arrange
            var userId = 4;

            // Act
            var result = await sutFailure.DeleteUser(userId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task AddRolesToUsers()
        {
            // Arrange
            var userId = 2;

            var user = context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == userId);

            var updateUserRoleRequest = new UpdateUserRoleRequest();
            updateUserRoleRequest.RoleIds.Add(3);

            // Act
            var result = await sut.AddUserRoles(userId, updateUserRoleRequest.RoleIds);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Roles Added"));
        }

        [Test]
        [Category("Services")]
        public async Task RemoveRolesFromUsers()
        {
            // Arrange
            var userId = 1;

            var user = context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == userId);

            var updateUserRoleRequest = new UpdateUserRoleRequest();
            updateUserRoleRequest.RoleIds.Add(3);

            // Act
            var result = await sut.RemoveUserRoles(userId, updateUserRoleRequest.RoleIds);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Roles Removed"));
        }

        [Test]
        [Category("Services")]
        public async Task ActivateUsers()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await sut.ActivateUser(userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Activated"));
        }

        [Test]
        [Category("Services")]
        public async Task DeactivateUsers()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await sut.DeactivateUser(userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Deactivated"));
        }
    }
}
