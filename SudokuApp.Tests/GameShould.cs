using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using SudokuApp.Models;

namespace SudokuApp.Tests {

    public class GameShould {

        private Mock<User> user;
        private Mock<SudokuMatrix> matrix;

        [SetUp]
        public void Setup() {
            
            user = new Mock<User>();
            matrix = new Mock<SudokuMatrix>();

            matrix.Setup(m => m.IsSolved()).Returns(true);
        }

        [Test]
        [Category("Models")]
        public void HaveAWorkingConstructor() {
            
            // Arrange and Act
            Game game = new Game(user.Object, matrix.Object, Difficulty.TEST);

            // Assert
            Assert.IsNotNull(game);
        }

        [Test]
        [Category("Models")]
        public void ReturnTrueIfSolved() {

            // Arrange and Act
            Game game = new Game(user.Object, matrix.Object, Difficulty.TEST);

            // Assert
            Assert.That(game.IsSolved(), Is.EqualTo(true));
        }
    }
}