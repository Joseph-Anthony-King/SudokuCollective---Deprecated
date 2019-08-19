using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using SudokuApp.Utilities;
using SudokuApp.Models;

namespace SudokuApp.Tests {

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
        public void AcceptStringInConstructor() {

            var sut = new SudokuMatrix(stringList);

            Assert.That(stringList, Is.TypeOf<string>());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.SudokuCells.Count, Is.GreaterThan(0));
        }

        [Test]
        public void AcceptIntListInConstructor() {

            var sut = new SudokuMatrix(intList);

            Assert.That(intList, Is.TypeOf<List<int>>());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.SudokuCells.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CreateZeroedListWithBlankConstructor() {

            var sut = new SudokuMatrix();

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
        public void ReturnTrueIfValid() {

            var sut = populatedTestMatrix;

            Assert.That(sut.IsValid(), Is.True);
        }

        [Test]
        public void OutputValuesAsIntListWithToInt32List() {

            var sut = populatedTestMatrix;
            var result = sut.ToInt32List();

            Assert.That(result, Is.TypeOf<List<int>>());
            Assert.That(result.Count, Is.EqualTo(81));
        }

        [Test]
        public void OutputValuesAsIntListWithToDisplayedValuesList() {

            var sut = populatedTestMatrix;
            var result = sut.ToDisplayedValuesList();

            Assert.That(result, Is.TypeOf<List<int>>());
            Assert.That(result.Count, Is.EqualTo(81));
        }

        [Test]
        public void OutputValuesAsStringWithToString() {

            var sut = populatedTestMatrix;
            var result = sut.ToString();

            Assert.That(result, Is.TypeOf<string>());
            Assert.That(result.Length, Is.EqualTo(81));
        }

        [Test]
        public void HaveNoObscuredCellsOnTestDifficulty() {

            populatedTestMatrix.SetDifficulty(Difficulty.TEST);
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            Assert.That(result, Is.EqualTo(81));
        }

        [Test]
        public void Have35VisibleCellsOnEasyDifficulty() {

            populatedTestMatrix.SetDifficulty(Difficulty.EASY);
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            Assert.That(result, Is.EqualTo(35));
        }

        [Test]
        public void Have29VisibleCellsOnMediumDifficulty() {

            populatedTestMatrix.SetDifficulty(Difficulty.MEDIUM);
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            Assert.That(result, Is.EqualTo(29));
        }

        [Test]
        public void Have23VisibleCellsOnHardDifficulty() {

            populatedTestMatrix.SetDifficulty(Difficulty.HARD);
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            Assert.That(result, Is.EqualTo(23));
        }

        [Test]
        public void Have17VisibleCellsOnEvilDifficulty() {

            populatedTestMatrix.SetDifficulty(Difficulty.EVIL);
            var sut = populatedTestMatrix;
            var result = 0;

            foreach(var cell in sut.SudokuCells) {

                if (cell.Obscured == false) {

                    result++;
                }
            }

            Assert.That(result, Is.EqualTo(17));
        }

        [Test]
        public void DetermineIfMatrixIsSolved() {

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

            var sut = new SudokuMatrix(intList);
            sut.SetDifficulty(Difficulty.TEST);

            Assert.That(sut.IsSolved(), Is.True);
        }

        [Test]
        public void GenerateValidSolution() {

            var sut = new SudokuMatrix();
            sut.GenerateSolution();

            Assert.That(sut.IsValid(), Is.True);
        }
    }
}
