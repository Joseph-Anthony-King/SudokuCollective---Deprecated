using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
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
        public async Task GetAUser() {

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
        public async Task ReturnAMessageIfUserNotFound() {

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
            Assert.That(result.Users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task CreateAUser() {

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
            Assert.That(users.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Services")]
        public async Task CreateAnAdminUser() {

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
            Assert.That(users.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Services")]
        public async Task CreateUserWithUniqueUserName() {

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
        public async Task CreateUserWithUserNameRequired() {

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
        public async Task CreateUserWithUniqueEmail() {

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
        public async Task CreateUserWithEmailRequired() {

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
    }
}
