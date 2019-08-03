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
    }
}
