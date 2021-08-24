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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace SudokuCollective.Test.TestCases.Services
{
    public class AuthenticateServiceShould
    {
        private DatabaseContext context;
        private MockUsersRepository MockUsersRepository;
        private MockRolesRepository MockRolesRepository;
        private MockUserManagementService MockUserManagementService;
        private MockAppsRepository MockAppsRepository;
        private MockAppAdminsRepository MockAppAdminsRepository;
        private TokenManagement tokenManagement;
        private MemoryDistributedCache memoryCache;
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
            MockAppsRepository = new MockAppsRepository(context);
            MockAppAdminsRepository = new MockAppAdminsRepository(context);
            memoryCache = new MemoryDistributedCache(
                Options.Create(new MemoryDistributedCacheOptions()));

            MockUserManagementService = new MockUserManagementService();

            tokenManagement = new TokenManagement()
            {
                Secret = "3c1ad157-be37-40d2-9cc8-e7527a56aa7b",
                Issuer = "testProject",
                Audience = "testEnvironment"
            };

            IOptions<TokenManagement> options = Options.Create<TokenManagement>(tokenManagement);

            sutValid = new AuthenticateService(
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object, 
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object,
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockAppAdminsRepository.AppAdminsRepositorySuccessfulRequest.Object,
                MockUserManagementService.UserManagementServiceSuccssfulRequest.Object,
                options,
                memoryCache);
            sutInvalid = new AuthenticateService(
                MockUsersRepository.UsersRepositoryFailedRequest.Object,
                MockRolesRepository.RolesRepositoryFailedRequest.Object,
                MockAppsRepository.AppsRepositoryFailedRequest.Object,
                MockAppAdminsRepository.AppAdminsRepositoryFailedRequest.Object,
                MockUserManagementService.UserManagementServiceFailedRequest.Object, 
                options,
                memoryCache);
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
            Assert.That(result.IsSuccess, Is.True);
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
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not Found"));
            Assert.IsEmpty(result.Token);
            Assert.AreNotEqual(userName, result.User.UserName);
        }
    }
}
