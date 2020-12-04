using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class AppsServiceShould
    {
        private DatabaseContext context;
        private MockAppsRepository MockAppsRepository;
        private MockUsersRepository MockUsersRepository;
        private IAppsService sut;
        private IAppsService sutAppRepoFailure;
        private IAppsService sutUserRepoFailure;
        private DateTime dateCreated;
        private string license;
        private BaseRequest baseRequest;
        private PageListModel pageListModel;
        private int userId;
        private int appId;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            MockAppsRepository = new MockAppsRepository(context);
            MockUsersRepository = new MockUsersRepository(context);

            sut = new AppsService(
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object);

            sutAppRepoFailure = new AppsService(
                MockAppsRepository.AppsRepositoryFailedRequest.Object,
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object);

            sutUserRepoFailure = new AppsService(
                MockAppsRepository.AppsRepositorySuccessfulRequest.Object,
                MockUsersRepository.UsersRepositoryFailedRequest.Object);

            dateCreated = DateTime.UtcNow;
            license = TestObjects.GetLicense();
            baseRequest = TestObjects.GetBaseRequest();
            pageListModel = TestObjects.GetPageListModel();
            userId = 1;
            appId = 1;
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByID()
        {
            // Arrange

            // Act
            var result = await sut.GetApp(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Found"));
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByIDReturnsFalseIfNotFound()
        {
            // Arrange

            // Act
            var result = await sutAppRepoFailure.GetApp(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App Not Found"));
            Assert.That(result.App.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task GetMultipleApps()
        {
            // Arrange

            // Act
            var result = await sut.GetApps(new PageListModel());

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Apps Found"));
            Assert.That(result.Apps.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task CreateApps()
        {
            // Arrange

            // Act
            var result = await sut.CreateApp(new LicenseRequest()
            {

                Name = "Test App 3",
                OwnerId = 1,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            var apps = context.Apps.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Created"));
            Assert.That(result.App.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task NotCreateAppsIfOwnerDoesNotExist()
        {
            // Arrange

            // Act
            var result = await sutUserRepoFailure.CreateApp(new LicenseRequest()
            {

                Name = "Test App 3",
                OwnerId = 4,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            var apps = context.Apps.ToList();

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("User Does Not Exist"));
            Assert.That(apps.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByLicense()
        {
            // Arrange

            // Act
            var result = await sut.GetAppByLicense(license);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Found"));
            Assert.That(result.App.Id, Is.EqualTo(1));
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task NotGetAppByLicenseIfInvalid()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sutAppRepoFailure.GetAppByLicense(invalidLicense);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App Not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task RetrieveLicense()
        {
            // Arrange

            // Act
            var result = await sut.GetLicense(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Found"));
            Assert.That(result.License, Is.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task NotRetrieveLicenseIfAppDoesNotExist()
        {
            // Arrange

            // Act
            var result = await sutAppRepoFailure.GetLicense(5);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App Not Found"));
            Assert.That(result.License, Is.Not.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersByApp()
        {
            // Arrange

            // Act
            var result = await sut.GetAppUsers(1, pageListModel);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Users Found"));
            Assert.That(result.Users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateApps()
        {
            // Arrange

            // Act
            var result = await sut.UpdateApp(
                new AppRequest()
                {
                    License = license,
                    RequestorId = 1,
                    PageListModel = new PageListModel(),
                    Name = "Test App 1... UPDATED!",
                    DevUrl = "https://localhost:4200",
                    LiveUrl = "https://testapp.com"
                }
            );

            var name = result.App.Name;
            var apps = context.Apps.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Updated"));
            Assert.That(name, Is.EqualTo("Test App 1... UPDATED!"));
        }

        [Test]
        [Category("Services")]
        public async Task AddUsersToApp()
        {
            // Arrange

            // Act
            var result = await sut.AddAppUser(3, baseRequest);
            var appUsers = context.Users.Where(u => u.Apps.Any(ua => ua.AppId == 1)).ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Added To App"));
        }

        [Test]
        [Category("Services")]
        public async Task RemoveUsersFromApps()
        {
            // Arrange

            // Act
            var result = await sut.RemoveAppUser(2, baseRequest);
            var appUsers = (await sut.GetAppUsers(1, pageListModel)).Users;

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Removed From App"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteApps()
        {
            // Arrange

            // Act
            var result = await sut.DeleteOrResetApp(2);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Deleted"));
        }

        [Test]
        [Category("Services")]
        public async Task ActivateApps()
        {
            // Arrange

            // Act
            var result = await sut.ActivateApp(1);
            var app = context.Apps.FirstOrDefault(a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Activated"));
            Assert.That(app.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DeactivateApps()
        {
            // Arrange

            // Act
            var result = await sut.DeactivateApp(1);
            var app = context.Apps.FirstOrDefault(a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("App Deactivated"));
        }

        [Test]
        [Category("Services")]
        public async Task PermitValidRequests()
        {
            // Arrange

            // Act
            var result = await sut.IsRequestValidOnThisLicense(appId, license, userId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyInvalidLicenseRequests()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sutAppRepoFailure.IsRequestValidOnThisLicense(appId, invalidLicense, userId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task DenyRequestWhereUserIsNotRegisteredToApp()
        {
            // Arrange
            var user = new User()
            {
                Id = 4,
                UserName = "TestUser3",
                FirstName = "John",
                LastName = "Doe",
                NickName = "Johnny Boy",
                Email = "testuser3@example.com",
                Password = "password1",
                IsActive = true,
                DateCreated = dateCreated,
                DateUpdated = dateCreated
            };

            // Act
            var result = await sutUserRepoFailure.IsRequestValidOnThisLicense(appId, license, user.Id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task PermitSuperUserSystemWideAccess()
        {
            // Arrange
            var newAppResult = await sut.CreateApp(new LicenseRequest()
            {
                Name = "Test App 3",
                OwnerId = 2,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            var license = (sut.GetLicense(newAppResult.App.Id)).Result.License;

            var superUser = context.Users.Where(user => user.Id == 1).FirstOrDefault();

            // Act
            var superUserIsInApp = newAppResult.App.Users
                .Any(ua => ua.UserId == superUser.Id);

            var result = await sut.IsRequestValidOnThisLicense(appId, license, superUser.Id);

            // Assert
            Assert.That(superUserIsInApp, Is.False);
            Assert.That(superUser.IsSuperUser, Is.True);
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task PermitOwnerRequests()
        {
            // Arrange

            // Act
            var result = await sut.IsOwnerOfThisLicense(appId, license, userId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyInvalidOwnerRequests()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sutUserRepoFailure.IsOwnerOfThisLicense(appId, invalidLicense, userId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
