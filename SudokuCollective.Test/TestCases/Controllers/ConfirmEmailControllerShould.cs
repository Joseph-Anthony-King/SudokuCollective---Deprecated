using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class ConfirmEmailControllerShould
    {
        private DatabaseContext context;
        private ConfirmEmailController sutSuccess;
        private ConfirmEmailController sutFailure;
        private MockUsersService mockUsersService;
        private string emailConfirmationToken;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();

            mockUsersService = new MockUsersService(context);

            sutSuccess = new ConfirmEmailController(mockUsersService.UsersServiceSuccessfulRequest.Object);
            sutFailure = new ConfirmEmailController(mockUsersService.UsersServiceFailedRequest.Object);

            emailConfirmationToken = Guid.NewGuid().ToString();
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyConfirmUserEmails()
        {
            // Arrange

            // Act
            var result = sutSuccess.Index(emailConfirmationToken);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
        }

        [Test]
        [Category("Controllers")]
        public void ProcessRequestIfConfirmEmailTokenAlreadyProcessed()
        {
            // Arrange

            // Act
            var result = sutFailure.Index(emailConfirmationToken);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
        }
    }
}
