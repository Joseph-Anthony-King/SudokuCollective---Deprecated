using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.V1.Controllers;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class RegisterControllerShould
    {
        private DatabaseContext context;
        private RegisterController sutSuccess;
        private RegisterController sutFailure;
        private MockUsersService mockUsersService;
        private MockAuthenticateService mockAuthenticateService;
        private RegisterRequest registerRequest;
        private BaseRequest baseRequest;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            mockUsersService = new MockUsersService(context);
            mockAuthenticateService = new MockAuthenticateService();
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            sutSuccess = new RegisterController(
                mockUsersService.UsersServiceSuccessfulRequest.Object, 
                mockAuthenticateService.AuthenticateServiceSuccessfulRequest.Object, 
                mockWebHostEnvironment.Object);
            sutFailure = new RegisterController(
                mockUsersService.UsersServiceFailedRequest.Object, 
                mockAuthenticateService.AuthenticateServiceFailedRequest.Object,
                mockWebHostEnvironment.Object);

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
                Paginator = new Paginator()
            };

            baseRequest = TestObjects.GetBaseRequest();
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
            Assert.That(errorMessage, Is.EqualTo("Status Code 404: User not Created"));
            Assert.That(statusCode, Is.EqualTo(404));
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyResendEmailConfirmation()
        {
            // Arrange

            // Act
            var result = sutSuccess.ResendEmailConfirmation(baseRequest);
            var message = ((UserResult)((ObjectResult)result.Result).Value).Message;
            var statusCode = ((ObjectResult)result.Result).StatusCode;
            var emailResent = ((UserResult)((ObjectResult)result.Result).Value).ConfirmationEmailSuccessfullySent;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: Email Confirmation Email Resent"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(emailResent, Is.True);
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldSuccessfullyResendEmailConfirmationFail()
        {
            // Arrange

            // Act
            var result = sutFailure.ResendEmailConfirmation(baseRequest);
            var message = ((UserResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: Email Confirmation Email not Resent"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}
