using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Repositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Repositories
{
    public class AppAdminsRepositoryShould
    {
        private DatabaseContext context;
        private IAppAdminsRepository<AppAdmin> sut;
        private AppAdmin newAppAdmin;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            sut = new AppAdminsRepository<AppAdmin>(context);

            newAppAdmin = new AppAdmin()
            {
                AppId = 2,
                UserId = 2
            };
        }

        [Test]
        [Category("Repository")]
        public async Task CreateAppAdmins()
        {
            // Arrange

            // Act
            var result = await sut.Add(newAppAdmin);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((AppAdmin)result.Object, Is.InstanceOf<AppAdmin>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfCreateDifficutliesFails()
        {
            // Arrange
            newAppAdmin.Id = 1;

            // Act
            var result = await sut.Add(newAppAdmin);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAppAdminsById()
        {
            // Arrange

            // Act
            var result = await sut.GetById(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((AppAdmin)result.Object, Is.InstanceOf<AppAdmin>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetByIdFails()
        {
            // Arrange

            // Act
            var result = await sut.GetById(7);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAllAppAdmins()
        {
            // Arrange

            // Act
            var result = await sut.GetAll();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Objects.ConvertAll(aa => (AppAdmin)aa), Is.InstanceOf<List<AppAdmin>>());
        }

        [Test]
        [Category("Repository")]
        public async Task UpdateAppAdmins()
        {
            // Arrange
            var appAdmin = context.AppAdmins.FirstOrDefault(aa => aa.Id == 1);
            appAdmin.IsActive = !appAdmin.IsActive;

            // Act
            var result = await sut.Update(appAdmin);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Object, Is.InstanceOf<AppAdmin>());
            Assert.That(((AppAdmin)result.Object).IsActive, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfUpdateAppAdminsFails()
        {
            // Arrange

            // Act
            var result = await sut.Update(newAppAdmin);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task DeleteAppAdmins()
        {
            // Arrange
            var appAdmin = context
                .AppAdmins
                .FirstOrDefault(aa => aa.Id == 1);

            // Act
            var result = await sut.Delete(appAdmin);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfDeleteAppAdminsFails()
        {
            // Arrange

            // Act
            var result = await sut.Delete(newAppAdmin);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmItHasAnAppAdmin()
        {
            // Arrange

            // Act
            var result = await sut.HasEntity(1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmItHasAnAppAdminFails()
        {
            // Arrange
            var id = context
                .AppAdmins
                .ToList()
                .OrderBy(aa => aa.Id)
                .Last<AppAdmin>()
                .Id + 1;

            // Act
            var result = await sut.HasEntity(id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmItHasAnAdminRecord()
        {
            // Arrange
            var userId = context
                .Users
                .Where(u => u.Id == 1)
                .Select(u => u.Id)
                .FirstOrDefault();

            var appId = context
                .Apps
                .Where(a => a.Id == 1)
                .Select(a => a.Id)
                .FirstOrDefault();

            // Act
            var result = await sut.HasAdminRecord(appId, userId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfConfirmItHasAnAdminRecordFails()
        {
            // Arrange
            var userId = context
                .Users
                .Where(u => u.Id == 1)
                .Select(u => u.Id)
                .FirstOrDefault();

            var appId = context
                .Apps
                .Where(a => a.Id == 1)
                .Select(a => a.Id)
                .FirstOrDefault();

            // Act
            var result = await sut.HasAdminRecord(appId, userId + 1);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
