using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Utilities;
using SudokuCollective.Domain.Enums;

namespace SudokuCollective.Tests {

    public class UtilitiesShould {

        [Test]
        [Category("Utilities")]
        public void CreateASudokuCompliantIntList() {

            // Arrange
            var sut = UtilityMethods.GenerateSudokuCompliantIntList();

            // Act
            var result = new SudokuMatrix(sut);
            result.SetDifficulty(new Difficulty() {
                    Name = "Test", 
                    DifficultyLevel = DifficultyLevel.TEST
                }
            );

            // Assert
            Assert.That(sut.Count, Is.EqualTo(81));
            Assert.That(result.IsValid(), Is.EqualTo(true));
            Assert.That(result.IsSolved(), Is.EqualTo(true));
        }
    }
}
