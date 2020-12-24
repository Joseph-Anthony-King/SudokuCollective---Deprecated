﻿using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class UserManagementServiceShould
    {
        private DatabaseContext context;
        private MockUsersRepository MockUsersRepository;
        private IUserManagementService sut;
        private IUserManagementService sutFailure;
        private string userName;
        private string password;
        private string email;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            MockUsersRepository = new MockUsersRepository(context);

            sut = new UserManagementService(
                MockUsersRepository.UsersRepositorySuccessfulRequest.Object);
            sutFailure = new UserManagementService(
                MockUsersRepository.UsersRepositoryFailedRequest.Object);

            userName = "TestSuperUser";
            password = "password1";
            email = "TestSuperUser@example.com";
        }

        [Test]
        [Category("Services")]
        public async Task ConfirmUserIfValid()
        {
            // Arrange

            // Act
            var result = await sut.IsValidUser(userName, password);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyUserIfUserNameInvalid()
        {
            // Arrange
            var invalidUserName = "invalidUser";

            // Act
            var result = await sutFailure.IsValidUser(invalidUserName, password);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task DenyUserIfPasswordInvalid()
        {
            // Arrange
            var invalidPassword = "invalidPassword";

            // Act
            var result = await sut.IsValidUser(userName, invalidPassword);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task ReturnUserAuthenticationErrorTypeIfUserInvalid()
        {
            // Arrange
            var invalidUserName = "invalidUser";

            // Act
            var result = await sutFailure.ConfirmAuthenticationIssue(invalidUserName, password);

            // Assert
            Assert.That(result, Is.EqualTo(UserAuthenticationErrorType.USERNAMEINVALID));
        }

        [Test]
        [Category("Services")]
        public async Task ReturnPasswordAuthenticationErrorTypeIfPasswordInvalid()
        {
            // Arrange
            var invalidPassword = "invalidPassword";

            // Act
            var result = await sut.ConfirmAuthenticationIssue(userName, invalidPassword);

            // Assert
            Assert.That(result, Is.EqualTo(UserAuthenticationErrorType.PASSWORDINVALID));
        }

        [Test]
        [Category("Services")]
        public async Task ReturnUserNameIfEmailIsValid()
        {
            // Arrange

            // Act
            var result = await sut.ConfirmUserName(email);
            var success = result.Success;
            var username = result.UserName;

            // Assert
            Assert.That(success, Is.True);
            Assert.That(result, Is.InstanceOf<AuthenticationResult>());
            Assert.That(username, Is.EqualTo("TestSuperUser"));
        }

        [Test]
        [Category("Services")]
        public async Task ReturnMessageIfUserNameInvalid()
        {
            // Arrange
            var invalidEmail = "invalidEmail@example.com";

            // Act
            var result = await sutFailure.ConfirmUserName(invalidEmail);
            var success = result.Success;
            var message = result.Message;

            // Assert
            Assert.That(success, Is.False);
            Assert.That(result, Is.InstanceOf<AuthenticationResult>());
            Assert.That(message, Is.EqualTo("No User Is Using This Email"));
        }
    }
}
