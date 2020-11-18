using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class RegisterControllerShould
    {
        private DatabaseContext _context;
        private RegisterController sutSuccess;
        private RegisterController sutFailure;
        private MockUsersService mockUsersService;
        private RegisterRequest registerRequest;

        [SetUp]
        public async Task Setup()
        {
            _context = await TestDatabase.GetDatabaseContext();

            mockUsersService = new MockUsersService(_context);

            sutSuccess = new RegisterController(mockUsersService.UsersServiceSuccessfulRequest.Object);
            sutFailure = new RegisterController(mockUsersService.UsersServiceFailedRequest.Object);

            registerRequest = new RegisterRequest()
            {
                UserName = "TestUser3",
                FirstName = "Test",
                LastName = "User 3",
                NickName = "My Nickname",
                Email = "testuser3@example.com",
                Password = "password1",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyRegisterUsers()
        {
            // Arrange

            // Act
            var result = sutSuccess.SignUp(registerRequest);
            var message = ((UserResult)((ObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result.Result).StatusCode;
            var user = ((UserResult)((ObjectResult)result.Result.Result).Value).User;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(message, Is.EqualTo("Status Code 201: User Created"));
            Assert.That(statusCode, Is.EqualTo(201));
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyRegisterUsersFail()
        {
            // Arrange

            // Act
            var result = sutFailure.SignUp(registerRequest);
            var errorMessage = ((UserResult)((NotFoundObjectResult)result.Result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(errorMessage, Is.EqualTo("Status Code 404: User Not Created"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
