using System.Collections.Generic;
using NUnit.Framework;
using SudokuApp.Models;
using SudokuApp.Utilities;

namespace SudokuApp.Tests {

    public class SudokuCellShould {

        private List<int> intList;
        private int firstInt;
        private SudokuMatrix populatedTestMatrix;
        
        [SetUp]
        public void Setup() {

            intList = UtilityMethods.GenerateSudokuCompliantIntList();
            firstInt = intList[0];

            populatedTestMatrix = new SudokuMatrix(intList);
        }

        [Test]
        public void SetCoordinatesInConstructor() {

            var sut = populatedTestMatrix.SudokuCells[0];

            Assert.That(sut.Index, Is.EqualTo(1));
            Assert.That(sut.Column, Is.EqualTo(1));
            Assert.That(sut.Region, Is.EqualTo(1));
            Assert.That(sut.Row, Is.EqualTo(1));
        }

        [Test]
        public void HaveADefaultValueOfZero() {

            var testMatrix = new SudokuMatrix();
            var sut = testMatrix.SudokuCells[0];

            Assert.That(sut.Value, Is.EqualTo(0));
        }

        [Test]
        public void BeObscuredByDefault() {

            var sut = populatedTestMatrix.SudokuCells[0];

            Assert.That(sut.Obscured, Is.EqualTo(true));
            Assert.That(sut.DisplayValue, Is.EqualTo(0));
            Assert.That(sut.DisplayValue, Is.Not.EqualTo(sut.Value));
        }

        [Test]
        public void BeVisibleIfObscuredIsFalse() {

            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            Assert.That(firstInt, Is.Not.EqualTo(0));
            Assert.That(sut.DisplayValue, Is.EqualTo(firstInt));
        }

        [Test]
        public void HaveAvailableValuesNineCountIfValueIsZero() {

            var testMatrix = new SudokuMatrix();
            var sut = testMatrix.SudokuCells[0];

            Assert.That(sut.AvailableValues.Count, Is.EqualTo(9));
        }

        [Test]
        public void HaveAvailableValuesZeroCountIfValueNonZero() {

            var sut = populatedTestMatrix.SudokuCells[0];

            Assert.That(sut.AvailableValues.Count, Is.EqualTo(0));
        }

        [Test]
        public void ToInt32OutputValueAsInt() {

            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            Assert.That(sut.ToInt32(), Is.TypeOf<int>());
            Assert.That(sut.ToInt32(), Is.EqualTo(firstInt));
        }

        [Test]
        public void ToStringOutputValueAsString() {

            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            Assert.That(sut.ToString(), Is.TypeOf<string>());
            Assert.That(sut.ToString(), Is.EqualTo(firstInt.ToString()));
        }
    }
}
