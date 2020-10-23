using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class RolesControllerShould
    {
        private DatabaseContext context;
        private RolesController sutSuccess;
        private RolesController sutFailure;
        private MockRolesService mockRolesService;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private UpdateRoleRequest updateRoleRequest;
        private CreateRoleRequest createRoleRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockRolesService = new MockRolesService(context);
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            updateRoleRequest = new UpdateRoleRequest()
            {
                Id = 1,
                Name = "Test Role",
                RoleLevel = RoleLevel.NULL,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            createRoleRequest = new CreateRoleRequest()
            {
                Name = "Test Difficulty",
                RoleLevel = RoleLevel.NULL,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sutSuccess = new RolesController(
                mockRolesService.RolesServiceSuccessfulRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);

            sutFailure = new RolesController(
                mockRolesService.RolesServiceFailedRequest.Object,
                mockAppsService.AppsServiceSuccessfulRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetRole()
        {
            // Arrange
            var roleId = 1;

            // Act
            var result = sutSuccess.GetRole(roleId, baseRequest);
            var role = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(role, Is.InstanceOf<Role>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetRoleFail()
        {
            // Arrange
            var roleId = 2;

            // Act
            var result = sutFailure.GetRole(roleId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving role"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetRoles(baseRequest, true);
            var roles = ((OkObjectResult)result.Result.Result).Value;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Role>>>());
            Assert.That(((List<Role>)roles).Count, Is.EqualTo(4));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetRolesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetRoles(baseRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Role>>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error retrieving roles"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.PutRole(1, updateRoleRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateRolesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PutRole(1, updateRoleRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error updating role"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.PostRole(createRoleRequest);
            var difficulty = ((CreatedAtActionResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(difficulty, Is.InstanceOf<Role>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateRolesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PostRole(createRoleRequest);
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyDeleteRoles()
        {
            // Arrange
            var roleId = 1;

            // Act
            var result = sutSuccess.DeleteRole(roleId, baseRequest);
            var statusCode = ((OkResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldDeleteRolesFail()
        {
            // Arrange
            var roleId = 1;

            // Act
            var result = sutFailure.DeleteRole(roleId, baseRequest);
            var errorMessage = ((NotFoundObjectResult)result.Result).Value;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error deleting role"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
