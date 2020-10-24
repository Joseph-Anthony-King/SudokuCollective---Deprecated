using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SudokuCollective.Test.TestData;
using Microsoft.Extensions.Options;
using SudokuCollective.Data.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Core.Interfaces.APIModels.DTOModels;

namespace SudokuCollective.Test.TestCases.Services
{
    public class AuthenticateServiceShould
    {
        private DatabaseContext _context;
        private IAuthenticateService sutValid;
        private IAuthenticateService sutInvalid;
        private Mock<IUserManagementService> mockUserManagementServiceValid;
        private Mock<IUserManagementService> mockUserManagementServiceInvalid;
        private TokenManagement tokenManagement;
        private string userName;
        private string password;

        [SetUp]
        public async Task Setup()
        {
            userName = "TestSuperUser";
            password = "password1";

            _context = await TestDatabase.GetDatabaseContext();

            mockUserManagementServiceValid = new Mock<IUserManagementService>();
            mockUserManagementServiceValid
                .Setup(service => service.IsValidUser(userName, password))
                .Returns(Task.FromResult(true));

            mockUserManagementServiceInvalid = new Mock<IUserManagementService>();
            mockUserManagementServiceInvalid
                .Setup(service => service.IsValidUser(userName, password))
                .Returns(Task.FromResult(false));

            tokenManagement = new TokenManagement()
            {
                Secret = "3c1ad157-be37-40d2-9cc8-e7527a56aa7b",
                Issuer = "testProject",
                Audience = "testEnvironment",
                AccessExpiration = 30,
                RefreshExpiration = 60
            };

            IOptions<TokenManagement> options = Options.Create<TokenManagement>(tokenManagement);

            sutValid = new AuthenticateService(_context, mockUserManagementServiceValid.Object, options);
            sutInvalid = new AuthenticateService(_context, mockUserManagementServiceInvalid.Object, options);
        }

        [Test]
        [Category("Services")]
        public void AuthenticateUsersIfValidated()
        {
            // Arrange
            var tokenRequest = new TokenRequest()
            {
                UserName = userName,
                Password = password
            };

            // Act
            var result = sutValid.IsAuthenticated(
                tokenRequest,
                out string token,
                out IAuthenticatedUser user);

            // Assert
            Assert.That(result, Is.True);
            Assert.IsNotNull(token);
            Assert.AreEqual(userName, user.UserName);
        }

        [Test]
        [Category("Services")]
        public void RejectUsersIfNotValidated()
        {
            // Arrange
            var tokenRequest = new TokenRequest()
            {
                UserName = userName,
                Password = password
            };

            // Act
            var result = sutInvalid.IsAuthenticated(tokenRequest,
                out string token,
                out IAuthenticatedUser user);

            // Assert
            Assert.That(result, Is.False);
            Assert.IsEmpty(token);
            Assert.AreNotEqual(userName, user.UserName);
        }
    }
}
