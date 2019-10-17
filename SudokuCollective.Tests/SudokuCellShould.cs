using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Domain.Interfaces;
using SudokuCollective.Domain.Models;
using SudokuCollective.Domain.Utilities;

namespace SudokuCollective.Tests {

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
        [Category("Models")]
        public void ImplementIDBEntry() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }
        
        [Test]
        [Category("Models")]
        public void HaveAnID() {

            // Arrange and Act
           var sut = populatedTestMatrix.SudokuCells[0];
           
            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedMatrix() {

            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            var sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.SudokuMatrix, Is.TypeOf<SudokuMatrix>());
            Assert.That(sut.SudokuMatrixId, Is.TypeOf<int>());
        }

        [Test]
        [Category("Models")]
        public void SetCoordinatesInConstructor() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Index, Is.EqualTo(1));
            Assert.That(sut.Column, Is.EqualTo(1));
            Assert.That(sut.Region, Is.EqualTo(1));
            Assert.That(sut.Row, Is.EqualTo(1));
        }

        [Test]
        [Category("Models")]
        public void HaveADefaultValueOfZero() {

            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            var sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Value, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void BeObscuredByDefault() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Obscured, Is.EqualTo(true));
            Assert.That(sut.DisplayValue, Is.EqualTo(0));
            Assert.That(sut.DisplayValue, Is.Not.EqualTo(sut.Value));
        }

        [Test]
        [Category("Models")]
        public void BeVisibleIfObscuredIsFalse() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            // Assert
            Assert.That(firstInt, Is.Not.EqualTo(0));
            Assert.That(sut.DisplayValue, Is.EqualTo(firstInt));
            Assert.That(sut.DisplayValue, Is.EqualTo(sut.Value));
        }

        [Test]
        [Category("Models")]
        public void HaveAvailableValuesNineCountIfValueIsZero() {

            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            var sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.AvailableValues.Count, Is.EqualTo(9));
        }

        [Test]
        [Category("Models")]
        public void HaveAvailableValuesZeroCountIfValueNonZero() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.AvailableValues.Count, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveToInt32OutputValueAsInt() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            // Assert
            Assert.That(sut.ToInt32(), Is.TypeOf<int>());
            Assert.That(sut.ToInt32(), Is.EqualTo(firstInt));
        }

        [Test]
        [Category("Models")]
        public void HaveToStringOutputValueAsString() {

            // Arrange and Act
            var sut = populatedTestMatrix.SudokuCells[0];
            sut.Obscured = false;

            // Assert
            Assert.That(sut.ToString(), Is.TypeOf<string>());
            Assert.That(sut.ToString(), Is.EqualTo(firstInt.ToString()));
        }
    }
}
