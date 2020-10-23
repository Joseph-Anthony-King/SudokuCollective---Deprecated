using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Api.Controllers;

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
            var message = ((OkObjectResult)result).Value;

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(message, Is.EqualTo("Hello World from Sudoku Collective!"));
        }
    }
}
