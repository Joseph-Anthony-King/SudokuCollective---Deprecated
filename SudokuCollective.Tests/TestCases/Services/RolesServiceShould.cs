using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Services;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Services {

    public class RolesServiceShould {

        private DatabaseContext _context;
        private IRolesService sut;
        private string license;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();
            sut = new RolesService(_context);
            license = "D17F0ED3-BE9A-450A-A146-F6733DB2BBDB";
        }

        [Test]
        [Category("Services")]
        public async Task GetARole() {

            // Arrange

            // Act
            var result = await sut.GetRole(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Role, Is.TypeOf<Role>());
        }

        [Test]
        [Category("Services")]
        public async Task GetRoles() {

            // Arrange

            // Act
            var result = await sut.GetRoles();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Roles.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task GetRolesWithoutNullOrSuperUserRoleLevel() {

            // Arrange

            // Act
            var result = await sut.GetRoles();
            var nullAndSuperUserRoleLevelsBlocked = result.Roles
                .Any(role =>
                    role.RoleLevel.Equals(RoleLevel.NULL)
                    || role.RoleLevel.Equals(RoleLevel.SUPERUSER));

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Roles.Count, Is.EqualTo(2));
            Assert.That(nullAndSuperUserRoleLevelsBlocked, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task CreateARole() {

            // Arrange

            // Act
            var result = await sut.CreateRole(
                "testRole", RoleLevel.NULL);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Role, Is.TypeOf<Role>());
        }

        [Test]
        [Category("Services")]
        public async Task UpdateADifficulty() {

            // Arrange
            var updateRoleRequest = new UpdateRoleRequest() {

                Id = 1,
                Name = "Null UPDATED!",
                RoleLevel = RoleLevel.NULL,                
                License = license,
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            // Act
            var result = await sut.UpdateRole(1, updateRoleRequest);
            var updatedDifficulty = _context.Roles
                .FirstOrDefault(predicate: role => role.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(updatedDifficulty.Name, Is.EqualTo("Null UPDATED!"));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteADifficulty() {

            // Arrange

            // Act
            var result = await sut.DeleteRole(1);
            var roles = _context.Roles.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(roles.Count, Is.EqualTo(3));
        }
    }
}
