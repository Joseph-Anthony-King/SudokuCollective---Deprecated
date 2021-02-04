using NUnit.Framework;
using Moq;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using System.Collections.Generic;
using System;
using System.Threading;

namespace SudokuCollective.Test.TestCases.Models
{
    public class GameShould
    {
        private Game sut;
        private Mock<User> user;
        private Mock<SudokuMatrix> matrix;

        [SetUp]
        public void Setup()
        {
            user = new Mock<User>();
            matrix = new Mock<SudokuMatrix>();

            user.Setup(u => u.Games).Returns(new List<Game>());
            matrix.Setup(m => m.SudokuCells).Returns(new List<SudokuCell>());
            matrix.Setup(m => m.IsSolved()).Returns(true);

            sut = new Game(
                user.Object,
                matrix.Object,
                new Difficulty()
                {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );
        }

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveAWorkingConstructor()
        {
            // Arrange and Act

            // Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedMatrix()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.SudokuMatrix, Is.InstanceOf<SudokuMatrix>());
            Assert.That(sut.SudokuMatrixId, Is.InstanceOf<int>());
            Assert.That(sut.SudokuMatrixId, Is.EqualTo(sut.SudokuMatrix.Id));
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedSolution()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.SudokuSolution, Is.TypeOf<SudokuSolution>());
            Assert.That(sut.SudokuSolutionId, Is.InstanceOf<int>());
            Assert.That(sut.SudokuSolutionId, Is.EqualTo(sut.SudokuSolution.Id));
        }

        [Test]
        [Category("Models")]
        public void HasAReferenceToTheHostingApp()
        {
            // Arrange and Act

            // Asser
            Assert.That(sut.AppId, Is.InstanceOf<int>());
        }

        [Test]
        [Category("Models")]
        public void ContinueGameFieldDefaultsToTrue()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.ContinueGame, Is.InstanceOf<bool>());
            Assert.That(sut.ContinueGame, Is.True);
        }

        [Test]
        [Category("Models")]
        public void ReturnTrueIfSolved()
        {
            // Arrange

            // Act
            sut.KeepScore = true;
            sut.SudokuMatrix.Stopwatch.Start();
            Thread.Sleep(10000);
            sut.SudokuMatrix.Stopwatch.Stop();
            sut.IsSolved();

            // Assert
            Assert.That(sut.ContinueGame, Is.False);
            Assert.That(sut.Score, Is.GreaterThan(0));
            Assert.That(sut.TimeToSolve, Is.GreaterThan(new TimeSpan(0, 0, 0)));
            Assert.That(sut.DateCompleted, Is.GreaterThan(DateTime.MinValue));
            Assert.That(sut.DateUpdated, Is.GreaterThan(DateTime.MinValue));
            Assert.That(sut.DateUpdated, Is.EqualTo(sut.DateCompleted));
        }
    }
}
