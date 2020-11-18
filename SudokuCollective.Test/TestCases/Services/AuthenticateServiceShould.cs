using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class AuthenticateServiceShould
    {
        private DatabaseContext context;
        private MockUsersRepository MockUsersRepository;
        private MockRolesRepository MockRolesRepository;
        private MockUserManagementService MockUserManagementService;
        private TokenManagement tokenManagement;
        private IAuthenticateService sutValid;
        private IAuthenticateService sutInvalid;
        private string userName;
        private string password;

        [SetUp]
        public async Task Setup()
        {
            userName = "TestSuperUser";
            password = "password1";

            context = await TestDatabase.GetDatabaseContext();

            MockUsersRepository = new MockUsersRepository(context);
            MockRolesRepository = new MockRolesRepository(context);

            MockUserManagementService = new MockUserManagementService();

            tokenManagement = new TokenManagement()
            {
                Secret = "3c1ad157-be37-40d2-9cc8-e7527a56aa7b",
                Issuer = "testProject",
                Audience = "testEnvironment",
                AccessExpiration = 30,
                RefreshExpiration = 60
            };

            IOptions<TokenManagement> options = Options.Create<TokenManagement>(tokenManagement);

            sutValid = new AuthenticateService(
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object, 
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object, 
                MockUserManagementService.UserManagementServiceSuccssfulRequest.Object,
                options);
            sutInvalid = new AuthenticateService(
                MockUsersRepository.UsersRepositoryFailedRequest.Object,
                MockRolesRepository.RolesRepositoryFailedRequest.Object,
                MockUserManagementService.UserManagementServiceFailedRequest.Object, 
                options);
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
            var result = (sutValid.IsAuthenticated(tokenRequest)).Result;

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Found"));
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(userName, result.User.UserName);
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
            var result = (sutInvalid.IsAuthenticated(tokenRequest)).Result;

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Not Found"));
            Assert.IsEmpty(result.Token);
            Assert.AreNotEqual(userName, result.User.UserName);
        }
    }
}
