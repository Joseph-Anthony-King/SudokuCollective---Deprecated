using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class RolesServiceShould
    {
        private DatabaseContext context;
        private MockRolesRepository MockRolesRepository;
        private MemoryDistributedCache memoryCache;
        private RolesService sut;
        private RolesService sutFailue;
        private string license;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            MockRolesRepository = new MockRolesRepository(context);
            memoryCache = new MemoryDistributedCache(
                Options.Create(new MemoryDistributedCacheOptions()));

            sut = new RolesService(
                MockRolesRepository.RolesRepositorySuccessfulRequest.Object,
                memoryCache);
            sutFailue = new RolesService(
                MockRolesRepository.RolesRepositoryFailedRequest.Object,
                memoryCache);
            license = TestObjects.GetLicense();
        }

        [Test]
        [Category("Services")]
        public async Task GetARole()
        {
            // Arrange

            // Act
            var result = await sut.Get(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role Found"));
            Assert.That(result.Role, Is.TypeOf<Role>());
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfRoleNotFound()
        {
            // Arrange

            // Act
            var result = await sutFailue.Get(1);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Role not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task GetRoles()
        {
            // Arrange

            // Act
            var result = await sut.GetRoles();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Roles Found"));
            Assert.That(result.Roles, Is.TypeOf<List<IRole>>());
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfRolesNotFound()
        {
            // Arrange

            // Act
            var result = await sutFailue.GetRoles();

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Roles not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task GetRolesWithoutNullOrSuperUserRoleLevel()
        {
            // Arrange

            // Act
            var result = await sut.GetRoles();
            var nullAndSuperUserRoleLevelsBlocked = result.Roles
                .Any(role =>
                    role.RoleLevel.Equals(RoleLevel.NULL)
                    || role.RoleLevel.Equals(RoleLevel.SUPERUSER));

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Roles Found"));
            Assert.That(nullAndSuperUserRoleLevelsBlocked, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task UpdateADifficulty()
        {
            // Arrange
            var updateRoleRequest = new UpdateRoleRequest()
            {

                Id = 1,
                Name = "Null UPDATED!",
                RoleLevel = RoleLevel.NULL,
                License = license,
                RequestorId = 1,
                Paginator = new Paginator()
            };

            // Act
            var result = await sut.Update(1, updateRoleRequest);
            var updatedDifficulty = context.Roles
                .FirstOrDefault(role => role.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role Updated"));
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfUpdateFails()
        {
            // Arrange
            var updateRoleRequest = new UpdateRoleRequest()
            {

                Id = 10,
                Name = "Null UPDATED!",
                RoleLevel = RoleLevel.NULL,
                License = license,
                RequestorId = 1,
                Paginator = new Paginator()
            };

            // Act
            var result = await sutFailue.Update(1, updateRoleRequest);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Role not Found"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteADifficulty()
        {
            // Arrange

            // Act
            var result = await sut.Delete(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role Deleted"));
        }

        [Test]
        [Category("Services")]
        public async Task IssueMessageIfRoleNotDeleted()
        {
            // Arrange

            // Act
            var result = await sutFailue.Delete(10);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Role not Found"));
        }
    }
}
