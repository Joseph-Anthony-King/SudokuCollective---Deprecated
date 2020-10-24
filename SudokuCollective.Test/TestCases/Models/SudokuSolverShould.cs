using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.TestCases.Models
{
    public class SudokuSolverShould
    {
        [Test]
        [Category("Utilities")]
        public void HaveAConstructorWhichAcceptsIntList()
        {
            // Arrange and Act
            var intList = new List<int>() {
                    4, 1, 9, 2, 6, 5, 3, 8, 7,
                    2, 8, 3, 1, 7, 9, 4, 5, 6,
                    5, 6, 7, 4, 3, 8, 9, 1, 2,
                    1, 2, 5, 3, 9, 4, 7, 6, 8,
                    7, 3, 8, 5, 1, 6, 2, 4, 9,
                    6, 9, 4, 7, 8, 2, 5, 3, 1,
                    3, 5, 6, 8, 2, 7, 1, 9, 4,
                    8, 7, 1, 9, 4, 3, 6, 2, 5,
                    9, 4, 2, 6, 5, 1, 8, 7, 3
                };

            var sut = new SudokuSolver(intList);

            // Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        [Category("Utilities")]
        public void HaveAConstructorWhichAcceptsAString()
        {
            // Arrange and Act
            var intString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                "419265387",
                "283179456",
                "567438912",
                "125394768",
                "738516249",
                "694782531",
                "356827194",
                "871943625",
                "942651873");

            var sut = new SudokuSolver(intString);

            // Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        [Category("Utilities")]
        public async void SolveSudokuMatrices()
        {
            // Arrange
            var intList = new List<int>() {
                    4, 1, 9, 2, 6, 5, 3, 8, 7,
                    2, 8, 3, 1, 7, 9, 4, 5, 6,
                    5, 6, 7, 4, 3, 8, 9, 1, 2,
                    1, 2, 5, 3, 9, 4, 7, 6, 8,
                    7, 3, 8, 5, 1, 6, 2, 4, 9,
                    6, 9, 4, 7, 8, 2, 5, 3, 1,
                    3, 5, 6, 8, 2, 7, 1, 9, 4,
                    8, 7, 1, 9, 4, 3, 6, 2, 5,
                    9, 4, 2, 6, 5, 1, 8, 7, 3
                };

            var sut = new SudokuSolver(intList);
            sut.SetDifficulty(new Difficulty()
            {
                Name = "Easy",
                DifficultyLevel = DifficultyLevel.EASY
            }
            );

            // Act
            await sut.Solve();

            // Assert
            Assert.That(sut.IsValid(), Is.EqualTo(true));
        }
    }
}
