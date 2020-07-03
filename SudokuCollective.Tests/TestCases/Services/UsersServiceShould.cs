using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Services;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Services
{

    public class UsersServiceShould {

        private DatabaseContext _context;
        private IUsersService sut;
        private DateTime dateCreated;
        private string license;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();
            sut = new UsersService(_context);
            dateCreated = DateTime.UtcNow;
            license = TestObjects.GetLicense();
            baseRequest = TestObjects.GetBaseRequest();
        }

        [Test]
        [Category("Services")]
        public async Task GetUser() {

            // Arrange
            var userId = 1;

            // Act
            var result = await sut.GetUser(userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.User, Is.TypeOf<User>());
        }

        [Test]
        [Category("Services")]
        public async Task ReturnMessageIfUserNotFound() {

            // Arrange
            var userId = 5;

            // Act
            var result = await sut.GetUser(userId);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found"));
            Assert.That(result.User, Is.TypeOf<User>());
            Assert.That(result.User.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Services")]
        public async Task GetUsers() {

            // Arrange

            // Act
            var result = await sut.GetUsers(baseRequest.PageListModel);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Users.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Services")]
        public async Task CreateUser() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateUser(registerRequest);
            var users = _context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.User, Is.TypeOf<User>());
            Assert.That(users.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Services")]
        public async Task CreateAdminUser() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            var provideAdminRole = true;

            // Act
            var result = await sut.CreateUser(registerRequest, provideAdminRole);
            var users = _context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.User, Is.TypeOf<User>());
            Assert.That(result.User.IsAdmin, Is.True);
            Assert.That(users.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserNameUnique() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = "TestUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateUser(registerRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username is not unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserName() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = null,
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateUser(registerRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username is required"));
        }

        [Test]
        [Category("Services")]
        public async Task RejectUserNameWithSpecialCharacters() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = "@#$%^&*",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = "newuser@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateUser(registerRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User name can contain alphanumeric charaters or the following (._-), user name contained invalid characters."));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueEmail() {

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

            // Act
            var result = await sut.CreateUser(registerRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email is not unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireEmail() {

            // Arrange
            var registerRequest = new RegisterRequest() {

                UserName = "NewUser",
                FirstName = "New",
                LastName = "User",
                NickName = "New Guy",
                Email = null,
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.CreateUser(registerRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email is required"));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateUser() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.User.UserName, Is.EqualTo("TestSuperUserUPDATED"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueUserNameForUpdates() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = "TestUser",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username is not unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUserNameForUpdates() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = null,
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username is required"));
        }

        [Test]
        [Category("Services")]
        public async Task RejectUserNamewithSpecialCharacters() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = "@#$%^&*",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestSuperUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User name can contain alphanumeric charaters or the following (._-), user name contained invalid characters."));
        }

        [Test]
        [Category("Services")]
        public async Task RequireUniqueEmailWithUpdates() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = "TestUser@example.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email is not unique"));
        }

        [Test]
        [Category("Services")]
        public async Task RequireEmailWithUpdates() {

            // Arrange
            var userId = 1;

            var updateUserRequest = new UpdateUserRequest() {

                UserName = "TestSuperUserUPDATED",
                FirstName = "Test Super",
                LastName = "User",
                NickName = "Test Super User",
                Email = null,
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdateUser(userId, updateUserRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Email is required"));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateUserPassword() {

            // Arrange
            var userId = 1;

            var updatePasswordRequest = new UpdatePasswordRequest() {

                OldPassword = "password1",
                NewPassword = "password2",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdatePassword(userId, updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task RequireOldPaswordForUpdateRequests() {

            // Arrange
            var userId = 1;

            var updatePasswordRequest = new UpdatePasswordRequest() {

                OldPassword = "password!",
                NewPassword = "password2",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdatePassword(userId, updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Old password was incorrect"));
        }

        [Test]
        [Category("Services")]
        public async Task RejectPasswordUpdatesIfUserNotFound() {

            // Arrange
            var userId = 4;

            var updatePasswordRequest = new UpdatePasswordRequest() {

                OldPassword = "password1",
                NewPassword = "password2",
                License = TestObjects.GetLicense(),
                RequestorId = 1
            };

            // Act
            var result = await sut.UpdatePassword(userId, updatePasswordRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteUsers() {

            // Arrange
            var userId = 1;

            // Act
            var result = await sut.DeleteUser(userId);
            var users = _context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task ReturnErrorMessageIfUserNotFoundForDeletion() {

            // Arrange
            var userId = 4;

            // Act
            var result = await sut.DeleteUser(userId);
            var users = _context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found"));
        }

        [Test]
        [Category("Services")]
        public async Task AddRolesToUsers() {

            // Arrange
            var userId = 2;

            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(predicate: u => u.Id == userId);

            var preEditNumberOfRoles = user.Roles.Count;

            var updateUserRoleRequest = new UpdateUserRoleRequest();
            updateUserRoleRequest.RoleIds.Add(3);

            // Act
            var result = await sut.AddUserRoles(userId, updateUserRoleRequest.RoleIds);

            var postEditNumberOfRoles = user.Roles.Count;

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(preEditNumberOfRoles, Is.EqualTo(1));
            Assert.That(postEditNumberOfRoles, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task RemoveRolesFromUsers() {

            // Arrange
            var userId = 1;

            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(predicate: u => u.Id == userId);

            var preEditNumberOfRoles = user.Roles.Count;

            var updateUserRoleRequest = new UpdateUserRoleRequest();
            updateUserRoleRequest.RoleIds.Add(3);

            // Act
            var result = await sut.RemoveUserRoles(userId, updateUserRoleRequest.RoleIds);

            var postEditNumberOfRoles = user.Roles.Count;

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(preEditNumberOfRoles, Is.EqualTo(3));
            Assert.That(postEditNumberOfRoles, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task ActivateUsers() {

            // Arrange
            var userId = 1;

            // Act
            var result = await sut.ActivateUser(userId);

            var user = _context.Users
                .FirstOrDefault(predicate: u => u.Id == userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(user.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DeactivateUsers() {

            // Arrange
            var userId = 1;

            // Act
            var result = await sut.DeactivateUser(userId);

            var user = _context.Users
                .FirstOrDefault(predicate: u => u.Id == userId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(user.IsActive, Is.False);
        }
    }
}
