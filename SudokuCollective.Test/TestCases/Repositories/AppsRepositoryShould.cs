using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Repositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Repositories
{
    public class AppsRepositoryShould
    {
        private DatabaseContext context;
        private IAppsRepository<App> sut;
        private App newApp;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            sut = new AppsRepository<App>(context);

            newApp = new App(
                "Test App 3",
                "6e32e987-13b9-43ab-aee5-9df659eeb6bd",
                1,
                "https://localhost:8080",
                "https://testapp3.com");
        }

        [Test]
        [Category("Repository")]
        public async Task CreateApps()
        {
            // Arrange

            // Act
            var result = await sut.Add(newApp);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((App)result.Object, Is.InstanceOf<App>());
        }

        [Test]
        [Category("Repository")]
        public async Task ThrowExceptionIfCreateAppsFails()
        {
            // Arrange
            newApp.OwnerId = 0;

            // Act
            var result = await sut.Add(newApp);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Exception, Is.Not.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAppsById()
        {
            // Arrange

            // Act
            var result = await sut.GetById(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((App)result.Object, Is.InstanceOf<App>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetByIdFails()
        {
            // Arrange

            // Act
            var result = await sut.GetById(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAppsByLicense()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.GetByLicense(license);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((App)result.Object, Is.InstanceOf<App>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetAppsByLicenseFails()
        {
            // Arrange
            var license = "aabe5bb5-cf60-4d7e-8bf1-f3686e4a1c4c";

            // Act
            var result = await sut.GetByLicense(license);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAllApps()
        {
            // Arrange

            // Act
            var result = await sut.GetAll();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Objects.ConvertAll(a => (App)a), Is.InstanceOf<List<App>>());
        }

        [Test]
        [Category("Repository")]
        public async Task GetUsersByApp()
        {
            // Arrange

            // Act
            var result = await sut.GetAppUsers(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Objects.ConvertAll(a => (User)a), Is.InstanceOf<List<User>>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetUsersByAppFails()
        {
            // Arrange

            // Act
            var result = await sut.GetAppUsers(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task UpdateApps()
        {
            // Arrange
            var app = context.Apps.FirstOrDefault(a => a.Id == 1);
            app.Name = string.Format("{0} UPDATED!", app.Name);

            // Act
            var result = await sut.Update(app);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Object, Is.InstanceOf<App>());
            Assert.That(((App)result.Object).Name, Is.EqualTo(app.Name));
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfUpdateAppsFails()
        {
            // Arrange

            // Act
            var result = await sut.Update(newApp);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task AddUsersToApps()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.AddAppUser(3, license);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfAddUsersToAppsFails()
        {
            // Arrange
            var license = "aabe5bb5-cf60-4d7e-8bf1-f3686e4a1c4c";

            // Act
            var result = await sut.AddAppUser(3, license);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task RemoveUsersFromApps()
        {
            // Arrange
            var user = context
                .Users
                .Include(u => u.Roles)
                .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                .ToList()
                .FirstOrDefault(u => u.Apps.Any(ua => ua.AppId == 1) && u.IsSuperUser == false);

            var license = context
                .Apps
                .FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.RemoveAppUser(user.Id, license);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfRemoveUsersFromAppsFails()
        {
            // Arrange
            var license = "aabe5bb5-cf60-4d7e-8bf1-f3686e4a1c4c";

            // Act
            var result = await sut.RemoveAppUser(2, license);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task DeleteApps()
        {
            // Arrange
            var app = context.Apps.FirstOrDefault(a => a.Id == 1);

            // Act
            var result = await sut.Delete(app);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfDeleteAppsFails()
        {
            // Arrange

            // Act
            var result = await sut.Delete(newApp);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ResetApps()
        {
            // Arrange
            var app = context.Apps.FirstOrDefault(a => a.Id == 1);

            // Act
            var result = await sut.Reset(app);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfResetAppsFails()
        {
            // Arrange

            // Act
            var result = await sut.Reset(newApp);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ActivateApps()
        {
            // Arrange

            // Act
            var result = await sut.Activate(1);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfActivateAppsFails()
        {
            // Arrange

            // Act
            var result = await sut.Activate(3);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task DeactivateApps()
        {
            // Arrange

            // Act
            var result = await sut.Deactivate(1);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfDeactivateAppsFails()
        {
            // Arrange

            // Act
            var result = await sut.Deactivate(3);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmItHasAnApp()
        {
            // Arrange

            // Act
            var result = await sut.HasEntity(1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmItHasAnAppFails()
        {
            // Arrange
            var id = context
                .Apps
                .ToList()
                .OrderBy(a => a.Id)
                .Last<App>()
                .Id + 1;

            // Act
            var result = await sut.HasEntity(id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmAppLicenseIsValid()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.IsAppLicenseValid(license);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmAppLicenseIsValidFails()
        {
            // Arrange

            // Act
            var result = await sut.IsAppLicenseValid("aabe5bb5-cf60-4d7e-8bf1-f3686e4a1c4c");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmIfUserIsRegisteredToApp()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.IsUserRegisteredToApp(1, license, 1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmIfUserIsRegisteredToAppFails()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.IsUserRegisteredToApp(1, license, 5);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmIfUserIsOwnerToApp()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.IsUserOwnerOfApp(1, license, 1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmIfUserIsOwnerToAppFails()
        {
            // Arrange
            var license = context.Apps.FirstOrDefault(a => a.Id == 1).License;

            // Act
            var result = await sut.IsUserOwnerOfApp(1, license, 2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task GetLicenses()
        {
            // Arrange
            var license = TestObjects.GetLicense();

            // Act
            var result = await sut.GetLicense(1);

            // Assert
            Assert.That(result, Is.InstanceOf<string>());
            Assert.That(result, Is.EqualTo(license));
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnNullIfGetLicensesFails()
        {
            // Arrange

            // Act
            var result = await sut.GetLicense(3);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetMyApps()
        {
            // Arrange

            // Act
            var result = await sut.GetMyApps(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Objects.ConvertAll(a => (App)a), Is.InstanceOf<List<App>>());
        }
    }
}
