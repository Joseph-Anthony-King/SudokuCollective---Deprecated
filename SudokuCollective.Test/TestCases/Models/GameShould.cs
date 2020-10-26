using NUnit.Framework;
using Moq;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using System.Collections.Generic;

namespace SudokuCollective.Test.TestCases.Models
{
    public class GameShould
    {
        private Mock<User> user;
        private Mock<SudokuMatrix> matrix;

        [SetUp]
        public void Setup()
        {

            user = new Mock<User>();
            matrix = new Mock<SudokuMatrix>();

            user.Setup(u => u.Games).Returns(new List<Game>());
            matrix.Setup(u => u.SudokuCells).Returns(new List<SudokuCell>());

            matrix.Setup(m => m.IsSolved()).Returns(true);
        }

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry()
        {
            // Arrange and Act
            Game sut = new Game(
                user.Object,
                matrix.Object,
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID()
        {
            // Arrange and Act
            Game sut = new Game(
                user.Object,
                matrix.Object,
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveAWorkingConstructor()
        {
            // Arrange and Act
            Game sut = new Game(
                new User(),
                new SudokuMatrix(),
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedMatrix()
        {
            // Arrange and Act
            Game sut = new Game(
                user.Object,
                new SudokuMatrix(),
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.SudokuMatrix, Is.TypeOf<SudokuMatrix>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedSolution()
        {
            // Arrange and Act
            Game sut = new Game(
                user.Object,
                matrix.Object,
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.SudokuSolution, Is.TypeOf<SudokuSolution>());
        }

        [Test]
        [Category("Models")]
        public void ReturnTrueIfSolved()
        {
            // Arrange and Act
            Game sut = new Game(
                user.Object,
                matrix.Object,
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.IsSolved(), Is.EqualTo(true));
        }
    }
}
