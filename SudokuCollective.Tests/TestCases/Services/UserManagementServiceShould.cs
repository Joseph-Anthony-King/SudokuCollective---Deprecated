using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Services;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Services {

    public class UserManagementServiceShould {

        private DatabaseContext _context;
        private IUserManagementService sut;
        private string userName;
        private string password;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();
            sut = new UserManagementService(_context);
            userName = "TestSuperUser";
            password = "password1";
        }

        [Test]
        [Category("Services")]
        public async Task ConfirmUserIfValid() {

            // Arrange

            // Act
            var result = await sut.IsValidUser(userName, password);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyUserIfUserNameInvalid() {

            // Arrange
            var invalidUserName = "invalidUser";

            // Act
            var result = await sut.IsValidUser(invalidUserName, password);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task DenyUserIfPasswordInvalid() {

            // Arrange
            var invalidPassword = "invalidPassword";

            // Act
            var result = await sut.IsValidUser(userName, invalidPassword);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task ReturnUserAuthenticationErrorTypeIfUserInvalid()
        {

            // Arrange
            var invalidUserName = "invalidUser";

            // Act
            var result = await sut.ConfirmAuthenticationIssue(invalidUserName, password);

            // Assert
            Assert.That(result, Is.EqualTo(UserAuthenticationErrorType.USERNAMEINVALID));
        }

        [Test]
        [Category("Services")]
        public async Task ReturnPasswordAuthenticationErrorTypeIfPasswordInvalid()
        {

            // Arrange
            var invalidPassword = "invalidPassword";

            // Act
            var result = await sut.ConfirmAuthenticationIssue(userName, invalidPassword);

            // Assert
            Assert.That(result, Is.EqualTo(UserAuthenticationErrorType.PASSWORDINVALID));
        }
    }
}
