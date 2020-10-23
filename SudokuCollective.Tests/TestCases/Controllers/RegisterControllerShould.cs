using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Domain.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;

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
            var result = sutSuccess.SignUp(registerRequest, true);
            var user = ((CreatedAtActionResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(user, Is.InstanceOf<User>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyRegisterUsersFail()
        {
            // Arrange

            // Act
            var result = sutFailure.SignUp(registerRequest, true);
            var errorMessage = ((NotFoundObjectResult)result.Result.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<User>>());
            Assert.That(errorMessage, Is.InstanceOf<string>());
            Assert.That(errorMessage, Is.EqualTo("Error creating user"));
        }
    }
}
