using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.ResultModels;

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
                Paginator = new Paginator()
            };

            createRoleRequest = new CreateRoleRequest()
            {
                Name = "Test Difficulty",
                RoleLevel = RoleLevel.NULL,
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                Paginator = new Paginator()
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
            var message = ((RoleResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;
            var role = ((RoleResult)((OkObjectResult)result.Result.Result).Value).Role;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Role Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(role, Is.InstanceOf<Role>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetRoleFail()
        {
            // Arrange
            var roleId = 2;

            // Act
            var result = sutFailure.GetRole(roleId, baseRequest);
            var message = ((RoleResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Role Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.GetRoles(baseRequest, true);
            var message = ((RolesResult)((OkObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Role>>>());
            Assert.That(message, Is.EqualTo("Status Code 200: Roles Found"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetRolesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.GetRoles(baseRequest, true);
            var message = ((RolesResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<Role>>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Roles Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyUpdateRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.PutRole(1, updateRoleRequest);
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Role Updated"));
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldUpdateRoleFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PutRole(1, updateRoleRequest);
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Role Not Updated"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyCreateRoles()
        {
            // Arrange

            // Act
            var result = sutSuccess.PostRole(createRoleRequest);
            var message = ((RoleResult)((ObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(message, Is.EqualTo("Status Code 201: Role Created"));
            Assert.That(statusCode, Is.EqualTo(201));
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldCreateRolesFail()
        {
            // Arrange

            // Act
            var result = sutFailure.PostRole(createRoleRequest);
            var message = ((RoleResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<Role>>());
            Assert.That(message, Is.EqualTo("Status Code 404: Role Not Created"));
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
            var message = ((BaseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Role Deleted"));
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
            var message = ((BaseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Role Not Deleted"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
