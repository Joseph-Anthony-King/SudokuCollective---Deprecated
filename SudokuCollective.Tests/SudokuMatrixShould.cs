using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Utilities;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;

namespace SudokuCollective.Tests {

    public class SudokuMatrixShould {

        private List<int> intList;
        private string stringList;
        private SudokuMatrix populatedTestMatrix;

        [SetUp]
        public void Setup() {

            intList = UtilityMethods.GenerateSudokuCompliantIntList();

            StringBuilder sb = new StringBuilder();

            foreach (var i in intList) {

                sb.Append(i.ToString());
            }

            stringList = sb.ToString();

            populatedTestMatrix = new SudokuMatrix(intList);
        }
        
        [Test]
        [Category("Models")]
        public void HaveAnID() {

            // Arrange and Act
            var sut = new SudokuMatrix(stringList);

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void AcceptStringInConstructor() {

            // Arrange and Act
            var sut = new SudokuMatrix(stringList);

            // Assert
            Assert.That(stringList, Is.TypeOf<string>());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.SudokuCells.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("Models")]
        public void AcceptIntListInConstructor() {

            // Arrange and Act
            var sut = new SudokuMatrix(intList);

            // Assert
            Assert.That(intList, Is.TypeOf<List<int>>());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.SudokuCells.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("Models")]
        public void CreateZeroedListWithBlankConstructor() {

            // Arrange and Act
            var sut = new SudokuMatrix();

            // Assert
            Assert.That(sut.SudokuCells[0].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[9].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[18].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[27].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[36].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[45].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[54].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[63].Value, Is.EqualTo(0));
            Assert.That(sut.SudokuCells[72].Value, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void ReturnTrueIfValid() {

            // Arrange and Act
            var sut = populatedTestMatrix;

            // Assert
            Assert.That(sut.IsValid(), Is.True);
        }

        [Test]
        [Category("Models")]
        public void OutputValuesAsIntListWithToInt32List() {

            // Arrange
            var sut = populatedTestMatrix;

            // Act
            var result = sut.ToInt32List();

            // Assert
            Assert.That(result, Is.TypeOf<List<int>>());
            Assert.That(result.Count, Is.EqualTo(81));
        }

        [Test]
        [Category("Models")]
        public void OutputValuesAsIntListWithToDisplayedValuesList() {

            // Arrange
            var sut = populatedTestMatrix;

            // Act
            var result = sut.ToDisplayedValuesList();

            // Assert
            Assert.That(result, Is.TypeOf<List<int>>());
            Assert.That(result.Count, Is.EqualTo(81));
        }

        [Test]
        [Category("Models")]
        public void OutputValuesAsStringWithToString() {

            // Arrange
            var sut = populatedTestMatrix;

            // Act
            var result = sut.ToString();

            // Assert
            Assert.That(result, Is.TypeOf<string>());
            Assert.That(result.Length, Is.EqualTo(81));
        }

        [Test]
        [Category("Models")]
        public void HaveNoObscuredCellsOnTestDifficulty() {

            // Arrange
            populatedTestMatrix.SetDifficulty(new Difficulty() {
                Name = "Test",
                DifficultyLevel = DifficultyLevel.TEST
            });

            // Act
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            // Assert
            Assert.That(result, Is.EqualTo(81));
        }

        [Test]
        [Category("Models")]
        public void Have35VisibleCellsOnEasyDifficulty() {

            // Arrange
            populatedTestMatrix.SetDifficulty(new Difficulty() {
                Name = "Easy",
                DifficultyLevel = DifficultyLevel.EASY
            });

            // Act
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            // Assert
            Assert.That(result, Is.EqualTo(35));
        }

        [Test]
        [Category("Models")]
        public void Have29VisibleCellsOnMediumDifficulty() {

            // Arrange
            populatedTestMatrix.SetDifficulty(new Difficulty() {
                Name = "Medium",
                DifficultyLevel = DifficultyLevel.MEDIUM
            });

            // Act
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            // Assert
            Assert.That(result, Is.EqualTo(29));
        }

        [Test]
        [Category("Models")]
        public void Have23VisibleCellsOnHardDifficulty() {

            // Arrange
            populatedTestMatrix.SetDifficulty(new Difficulty() {
                Name = "Hard",
                DifficultyLevel = DifficultyLevel.HARD
            });

            // Act
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            // Assert
            Assert.That(result, Is.EqualTo(23));
        }

        [Test]
        [Category("Models")]
        public void Have17VisibleCellsOnEvilDifficulty() {

            // Arrange
            populatedTestMatrix.SetDifficulty(new Difficulty() {
                Name = "Evil",
                DifficultyLevel = DifficultyLevel.EVIL
            });

            // Act
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            // Assert
            Assert.That(result, Is.EqualTo(17));
        }

        [Test]
        [Category("Models")]
        public void DetermineIfMatrixIsSolved() {

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

            // Act
            var sut = new SudokuMatrix(intList);
            sut.SetDifficulty(new Difficulty() {
                Name = "Test",
                DifficultyLevel = DifficultyLevel.TEST
            });

            // Assert
            Assert.That(sut.IsSolved(), Is.True);
        }

        [Test]
        [Category("Models")]
        public void GenerateValidSolutions() {

            // Arrange
            var sut = new SudokuMatrix();

            // Act
            sut.GenerateSolution();

            // Assert
            Assert.That(sut.IsValid(), Is.True);
        }
    }
}
