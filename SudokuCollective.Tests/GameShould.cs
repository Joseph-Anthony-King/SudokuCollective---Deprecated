using NUnit.Framework;
using Moq;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;

namespace SudokuCollective.Tests {

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
        public void HaveAnID() {

            // Arrange and Act
            Game sut = new Game(
                user.Object, 
                matrix.Object, 
                new Difficulty() { 
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
        public void HaveAWorkingConstructor() {
            
            // Arrange and Act
            Game sut = new Game(
                user.Object, 
                matrix.Object, 
                new Difficulty() { 
                    Name = "Test", 
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        [Category("Models")]
        public void ReturnTrueIfSolved() {

            // Arrange and Act
            Game sut = new Game(
                user.Object, 
                matrix.Object, 
                new Difficulty() { 
                    Name = "Test", 
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.IsSolved(), Is.EqualTo(true));
        }
    }
}
