using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class HelloWorldControllerShould
    {
        [Test]
        [Category("Controllers")]
        public void ReturnAMessage()
        {
            // Arrange
            var sut = new HelloWorldController();

            // Act
            var result = sut.Get();
            var success = ((BaseResult)((OkObjectResult)result).Value).Success;
            var message = ((BaseResult)((OkObjectResult)result).Value).Message;
            var statusCode = ((OkObjectResult)result).StatusCode;

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Status Code 200: Hello World from Sudoku Collective!"));
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
